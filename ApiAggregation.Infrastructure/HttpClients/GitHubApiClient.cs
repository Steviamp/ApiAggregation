using ApiAggregation.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace ApiAggregation.Infrastructure.HttpClients
{
    public class GithubApiClient : BaseApiClient
    {
        public GithubApiClient(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration)
        {
            var baseUrl = configuration["GithubApi:BaseUrl"];
            httpClient.BaseAddress = new Uri(baseUrl);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "API-Aggregation-Service");

            var token = configuration["GithubApi:Token"];
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }
        public async Task<GithubApiResponse> GetTrendingRepositoriesAsync()
        {
            var url = "search/repositories?q=stars:>1&sort=stars&order=desc";
            return await GetAsync<GithubApiResponse>(url);
        }
    }
}
