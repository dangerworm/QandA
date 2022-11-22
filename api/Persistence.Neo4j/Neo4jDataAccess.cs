using Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;

namespace Persistence.Neo4j
{
    public class Neo4jDataAccess : INeo4jDataAccess
    {
        private IAsyncSession _session;
        private ILogger<Neo4jDataAccess> _logger;

        private string _database;

        public Neo4jDataAccess(ILogger<Neo4jDataAccess> logger, IDriver driver, IConfiguration configuration)
        {
            _logger = logger;

            var applicationSettings = new ApplicationSettings();
            configuration.Bind(ApplicationSettings.SectionKey, applicationSettings);

            _database = applicationSettings.Neo4jDatabase;
            _session = driver.AsyncSession(configBuilder => configBuilder.WithDatabase(_database));
        }

        public async Task<IList<string>> ExecuteReadListAsync(string query, string returnObjectKey, IDictionary<string, object>? parameters = null)
        {
            return await ExecuteReadListAsync<string>(query, returnObjectKey, parameters);
        }

        public async Task<IList<IDictionary<string, object>>> ExecuteReadDictionaryAsync(string query, string returnObjectKey, IDictionary<string, object>? parameters = null)
        {
            return await ExecuteReadListAsync<IDictionary<string, object>>(query, returnObjectKey, parameters);
        }
     
        public async Task<T> ExecuteReadScalarAsync<T>(string query, IDictionary<string, object>? parameters = null)
        {
            try
            {
                parameters ??= new Dictionary<string, object>();

                return await _session.ExecuteReadAsync(async queryRunner =>
                    await RunAndReturnSingle<T>(queryRunner, query, parameters));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "There was a problem while executing a read query on the Neo4j database");
                throw;
            }
        }

        public async Task<T> ExecuteWriteTransactionAsync<T>(string query, IDictionary<string, object>? parameters = null)
        {
            try
            {
                parameters ??= new Dictionary<string, object>();

                return await _session.ExecuteWriteAsync(queryRunner =>
                    RunAndReturnSingle<T>(queryRunner, query, parameters));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "There was a problem while executing a write to the Neo4j database");
                throw;
            }
        }
        
        public async ValueTask DisposeAsync()
        {
            await _session.CloseAsync();
        }

        private static async Task<T> RunAndReturnSingle<T>(IAsyncQueryRunner queryRunner, string query, IDictionary<string, object>? parameters = null)
        {
            var result = await queryRunner.RunAsync(query, parameters);
            var keys = await result.KeysAsync();
            var records = await result.ToListAsync();

            return records.FirstOrDefault().As<T>();
        }

        private async Task<IList<T>> ExecuteReadListAsync<T>(string query, string returnObjectKey, IDictionary<string, object>? parameters)
        {
            try
            {
                parameters ??= new Dictionary<string, object>();

                var result = await _session.ExecuteReadAsync(async transaction =>
                {
                    var result = await transaction.RunAsync(query, parameters);
                    var records = await result.ToListAsync();

                    return records
                        .Select(record => (T)record.Values[returnObjectKey])
                        .ToList();
                });

                return result;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "There was a problem while executing a read query on the Neo4j database");
                throw;
            }
        }
    }
}
