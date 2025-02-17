namespace ApiAggregation.Domain.Interfaces
{
    public interface IExternalApiClient
    {
        string Name { get; }
        Task<T> FetchDataAsync<T>(IDictionary<string, string> parameters) where T : class;
    }
}
