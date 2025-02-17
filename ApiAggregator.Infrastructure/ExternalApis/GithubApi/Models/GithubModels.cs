using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAggregator.Infrastructure.ExternalApis.GithubApi.Models
{
    public class GitHubRepo
    {
        public string FullName { get; set; }
        public string Description { get; set; }
        public int StargazersCount { get; set; }
        public int ForksCount { get; set; }
        public int OpenIssuesCount { get; set; }
    }

    public class GitHubIssue
    {
        public int Number { get; set; }
        public string Title { get; set; }
        public string State { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class GitHubPullRequest
    {
        public int Number { get; set; }
        public string Title { get; set; }
        public string State { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class RepoData
    {
        public GitHubRepo Repository { get; set; }
        public IEnumerable<GitHubIssue> TopIssues { get; set; }
        public IEnumerable<GitHubPullRequest> TopPullRequests { get; set; }
    }
}
