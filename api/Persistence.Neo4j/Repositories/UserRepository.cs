using Common.Nodes;
using Microsoft.Extensions.Logging;
using static Persistence.Neo4j.NodeTypes;
using static Persistence.Neo4j.Relationships;

namespace Persistence.Neo4j.Repositories
{
    public class UserRepository : IUserRepository
    {
        private INeo4jDataAccess _neo4jDataAccess;
        private ILogger<UserRepository> _logger;

        public UserRepository(INeo4jDataAccess neo4jDataAccess, ILogger<UserRepository> logger)
        {
            _neo4jDataAccess = neo4jDataAccess;
            _logger = logger;
        }

        public async Task<object> Create(Guid accountId, UserNode user)
        {
            var query =
                $"MATCH (account:{ACCOUNT} {{ Id: '{accountId}' }}) " +
                $"MERGE (account)-[i:{IDENTIFIES}]->(user:{USER} {{ Id: '{user.Id}' }}) " +
                $"ON CREATE SET " +
                $"  i.Created = '{DateTime.UtcNow:u}', " +
                $"  i.IsDeleted = false, " +
                $"  user.Created = '{DateTime.UtcNow:u}', " +
                $"  user.IsDeleted = false, " +
                $"  user.{nameof(user.GivenName)} = ${nameof(user.GivenName)}, " +
                $"  user.{nameof(user.FamilyName)} = ${nameof(user.FamilyName)} " +
                $"RETURN account, i, user";

            IDictionary<string, object> parameters = new Dictionary<string, object>
            {
                { nameof(user.GivenName), user.GivenName },
                { nameof(user.FamilyName), user.FamilyName }
            };

            return await _neo4jDataAccess.ExecuteWriteTransactionAsync<object>(query, parameters);
        }
    }
}
