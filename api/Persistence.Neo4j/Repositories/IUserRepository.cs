using Common.Nodes;

namespace Persistence.Neo4j.Repositories
{
    public interface IUserRepository
    {
        Task<object> Create(Guid accountId, UserNode user);
    }
}