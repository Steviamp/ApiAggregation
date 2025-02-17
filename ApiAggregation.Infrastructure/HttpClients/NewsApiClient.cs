using ApiAggregation.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAggregation.Infrastructure.HttpClients
{
    public class NewsApiClient : BaseApiClient
    {
        public NewsApiClient(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration)
        {
            var baseUrl = configuration["NewsApi:BaseUrl"];
            httpClient.BaseAddress = new Uri(baseUrl);
        }

        public async Task<NewsApiResponse> GetNewsAsync()
        {
            var apiKey = _configuration["NewsApi:ApiKey"];
            var url = $"v2/top-headlines?country=us&apiKey={apiKey}";
            return await GetAsync<NewsApiResponse>(url);
        }
    }
}
