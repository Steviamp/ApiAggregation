using ApiAggregation.Infrastructure.HttpClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiAggregation.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHttpClient<WeatherApiClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["WeatherApi:BaseUrl"]);
            });

            services.AddHttpClient<NewsApiClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["NewsApi:BaseUrl"]);
            });

            services.AddHttpClient<GithubApiClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["GithubApi:BaseUrl"]);
                client.DefaultRequestHeaders.Add("User-Agent", "API-Aggregation-Service");
            });

            return services;
        }
    }
}
