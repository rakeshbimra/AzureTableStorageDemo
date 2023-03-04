using Azure.Data.Tables;

namespace AzureTableStorageDemo.WebApi.Helpers.AzureStorage
{
    public interface ITableStorage<T> where T : class, ITableEntity, new()
    {
        Task CreateTableIfNotExistsAsync();
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(string partitionKey, string rowKey);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(string partitionKey, string rowKey);
    }
}
