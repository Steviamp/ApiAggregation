using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAggregator.Infrastructure.ExternalApis.NewsApi.Models
{
    public class NewsApiResponse
    {
        public string Status { get; set; }
        public int TotalResults { get; set; }
        public IEnumerable<Article> Articles { get; set; }
    }

    public class Article
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public DateTime PublishedAt { get; set; }
    }

    public class NewsCategory
    {
        public string Category { get; set; }
        public IEnumerable<Article> Articles { get; set; }
    }
}
