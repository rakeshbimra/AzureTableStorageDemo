using Azure;
using Azure.Data.Tables;
using System.Linq;

namespace AzureTableStorageDemo.WebApi.Helpers.AzureStorage
{
    public class TableStorage<T> : ITableStorage<T> where T : class, ITableEntity, new()
    {
        private readonly ILogger<TableStorage<T>> _logger;
        private readonly ITableStorageClientFactory _tableStorageClientFactory;
        private readonly TableClient _client;

        public TableStorage(ILogger<TableStorage<T>> logger,
        ITableStorageClientFactory tableStorageClientFactory)
        {
            _logger = logger;
            _tableStorageClientFactory = tableStorageClientFactory;
            _client = _tableStorageClientFactory.CreateTableServiceClient().GetTableClient(typeof(T).Name);
        }

        public async Task CreateTableIfNotExistsAsync()
        {
            try
            {
                await _client.CreateIfNotExistsAsync();
                _logger.LogInformation($"Table '{typeof(T).Name}' created if not exists.");
            }

            catch (RequestFailedException ex)
            {
                _logger.LogError(ex, $"Failed to create table '{typeof(T).Name}'. Error: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var results = new List<T>();
            AsyncPageable<T> queryResults = _client.QueryAsync<T>();
            await foreach (T entity in queryResults)
            {
                results.Add(entity);
            }

            _logger.LogInformation($"Retrieved {results.Count} entities from table '{typeof(T).Name}'.");

            return results;
        }



        public async Task<T> GetAsync(string partitionKey, string rowKey)
        {
            try
            {
                Response<T> response = await _client.GetEntityAsync<T>(partitionKey, rowKey);
                _logger.LogInformation($"Retrieved entity with partition key '{partitionKey}' and row key '{rowKey}' from table '{typeof(T).Name}'.");
                return response.Value;
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                _logger.LogInformation($"Entity with partition key '{partitionKey}' and row key '{rowKey}' not found in table '{typeof(T).Name}'.");
                return null;
            }
            catch (RequestFailedException ex)
            {
                _logger.LogError(ex, $"Failed to retrieve entity with partition key '{partitionKey}' and row key '{rowKey}' from table '{typeof(T).Name}'. Error: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<T>> GetItemsAsync(string filter)
        {
            var results = new List<T>();

            // Execute the query
            var queryResults = _client.QueryAsync<T>(filter: filter);

            // Enumerate the query results and add them to the list
            await foreach (T entity in queryResults)
            {
                results.Add(entity);
            }

            _logger.LogInformation($"Retrieved {results.Count} entities from table '{typeof(T).Name}' with filter '{filter}'.");

            return results;
        }

        public async Task<T> AddAsync(T entity)
        {
            try
            {
                var response = await _client.AddEntityAsync(entity);

                _logger.LogInformation($"Added entity with partition key '{entity.PartitionKey}' and row key '{entity.RowKey}' to table '{typeof(T).Name}'.");
                return entity;
            }
            catch (RequestFailedException ex)
            {
                _logger.LogError(ex, $"Failed to add entity to table '{typeof(T).Name}'.");
                throw new Exception($"Failed to add entity. Error: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<T>> AddItemsAsync(IEnumerable<T> entities)
        {
            var results = new List<T>();
            foreach (var entity in entities)
            {
                try
                {
                    var response = await _client.AddEntityAsync(entity);

                    _logger.LogInformation($"Added entity with partition key '{entity.PartitionKey}' and row key '{entity.RowKey}' to table '{typeof(T).Name}'.");
                    results.Add(entity);
                }
                catch (RequestFailedException ex)
                {
                    _logger.LogError(ex, $"Failed to add entity to table '{typeof(T).Name}'.");
                    throw new Exception($"Failed to add entity. Error: {ex.Message}", ex);
                }
            }
            return results;
        }


        public async Task<T> UpdateAsync(T entity)
        {
            try
            {
                var response = await _client.UpdateEntityAsync(entity, entity.ETag, TableUpdateMode.Replace);
                _logger.LogInformation($"Updated entity with partition key '{entity.PartitionKey}' and row key '{entity.RowKey}'.");
                return entity;
            }
            catch (RequestFailedException ex)
            {
                _logger.LogError(ex, $"Failed to update entity with partition key '{entity.PartitionKey}' and row key '{entity.RowKey}'. Error: {ex.Message}");
                throw new Exception($"Failed to update entity with partition key '{entity.PartitionKey}' and row key '{entity.RowKey}'. Error: {ex.Message}", ex);
            }
        }

        public async Task DeleteAsync(string partitionKey, string rowKey)
        {
            try
            {
                await _client.DeleteEntityAsync(partitionKey, rowKey);
                _logger.LogInformation($"Deleted entity with partition key '{partitionKey}' and row key '{rowKey}'.");
            }
            catch (RequestFailedException ex)
            {
                _logger.LogError(ex, $"Failed to delete entity with partition key '{partitionKey}' and row key '{rowKey}'. Error: {ex.Message}");
                throw new Exception($"Failed to delete entity with partition key '{partitionKey}' and row key '{rowKey}'. Error: {ex.Message}", ex);
            }
        }

        public async Task DeleteItemsAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                try
                {
                    await _client.DeleteEntityAsync(entity.PartitionKey, entity.RowKey);
                    _logger.LogInformation($"Deleted entity with partition key '{entity.PartitionKey}' and row key '{entity.RowKey}' from table '{typeof(T).Name}'.");
                }
                catch (RequestFailedException ex)
                {
                    _logger.LogError(ex, $"Failed to delete entity with partition key '{entity.PartitionKey}' and row key '{entity.RowKey}' from table '{typeof(T).Name}'. Error: {ex.Message}");
                    throw new Exception($"Failed to delete entity with partition key '{entity.PartitionKey}' and row key '{entity.RowKey}'. Error: {ex.Message}", ex);
                }
            }
        }

    }
}
