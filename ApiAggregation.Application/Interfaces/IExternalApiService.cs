namespace ApiAggregation.Application.Interfaces
{
    public interface IExternalApiService<T>
    {
        Task<T> FetchDataAsync();
    }
}
