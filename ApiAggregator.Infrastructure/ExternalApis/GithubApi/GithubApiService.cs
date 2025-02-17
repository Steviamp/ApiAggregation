using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ApiAggregator.Infrastructure.ExternalApis.GithubApi
{
    public class GithubApiService : IExternalApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GithubApiService> _logger;
        private readonly IRequestStatisticsService _statisticsService;
        private readonly string _token;

        public string ApiName => "GithubApi";

        public GithubApiService(
            HttpClient httpClient,
            ILogger<GithubApiService> logger,
            IConfiguration configuration,
            IRequestStatisticsService statisticsService)
        {
            _httpClient = httpClient;
            _logger = logger;
            _statisticsService = statisticsService;
            _token = configuration["Github:Token"];

            _httpClient.BaseAddress = new Uri("https://api.github.com/");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "API-Aggregator");
            if (!string.IsNullOrEmpty(_token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _token);
            }
        }

        public async Task<AggregatedResult> FetchDataAsync(CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var repos = new[] { "dotnet/runtime", "microsoft/vscode", "kubernetes/kubernetes" };
                var tasks = repos.Select(repo => FetchRepoDataAsync(repo, cancellationToken));
                var repoData = await Task.WhenAll(tasks);

                stopwatch.Stop();
                await _statisticsService.RecordRequestAsync(ApiName, stopwatch.ElapsedMilliseconds);

                return new AggregatedResult
                {
                    Source = ApiName,
                    Type = "GitHub",
                    Data = new { Repositories = repoData },
                    Timestamp = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching GitHub data");
                throw;
            }
        }

        private async Task<RepoData> FetchRepoDataAsync(string repo, CancellationToken cancellationToken)
        {
            var repoTask = _httpClient.GetFromJsonAsync<GitHubRepo>($"repos/{repo}", cancellationToken);
            var issuesTask = _httpClient.GetFromJsonAsync<IEnumerable<GitHubIssue>>($"repos/{repo}/issues?state=open&per_page=5", cancellationToken);
            var pullsTask = _httpClient.GetFromJsonAsync<IEnumerable<GitHubPullRequest>>($"repos/{repo}/pulls?state=open&per_page=5", cancellationToken);

            await Task.WhenAll(repoTask, issuesTask, pullsTask);

            return new RepoData
            {
                Repository = await repoTask,
                TopIssues = await issuesTask,
                TopPullRequests = await pullsTask
            };
        }
    }
}
