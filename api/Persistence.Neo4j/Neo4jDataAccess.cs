using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4j.Driver;
using Microsoft.Extensions.Logging;

namespace Persistence.Neo4j
{
    public class Neo4jDataAccess : INeo4jDataAccess
    {
        private IAsyncSession _session;
        private ILogger<Neo4jDataAccess> _logger;

        private string _database;

        public Neo4jDataAccess(ILogger<Neo4jDataAccess> logger, IDriver driver, string database)
        {
            _logger = logger;
            
            _database = database;
            _session = driver.AsyncSession(configBuilder => configBuilder.WithDatabase(_database));
        }

        public async Task<IList<string>> ExecuteReadListAsync(string query, string returnObjectKey, IDictionary<string, object>? parameters = null)
        {
            return await ExecuteReadListAsync<string>(query, returnObjectKey, parameters);
        }

        public async Task<IList<Dictionary<string, object>>> ExecuteReadDictionaryAsync(string query, string returnObjectKey, IDictionary<string, object>? parameters = null)
        {
            return await ExecuteReadListAsync<Dictionary<string, object>>(query, returnObjectKey, parameters);
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

                return await _session.ExecuteWriteAsync(async queryRunner =>
                    await RunAndReturnSingle<T>(queryRunner, query, parameters));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "There was a problem while executing a read query on the Neo4j database");
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
            var record = await result.SingleAsync();

            return record[0].As<T>();
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
