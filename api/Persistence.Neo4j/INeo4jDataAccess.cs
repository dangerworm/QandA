namespace Persistence.Neo4j
{
    public interface INeo4jDataAccess : IAsyncDisposable
    {
        Task<IList<string>> ExecuteReadListAsync(string query, string returnObjectKey, IDictionary<string, object>? parameters = null);

        Task<IList<Dictionary<string, object>>> ExecuteReadDictionaryAsync(string query, string returnObjectKey, IDictionary<string, object>? parameters = null);

        Task<T> ExecuteReadScalarAsync<T>(string query, IDictionary<string, object>? parameters = null);

        Task<T> ExecuteWriteTransactionAsync<T>(string query, IDictionary<string, object>? parameters = null);
    }
}
