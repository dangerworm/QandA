using Common.Converters;
using Common.ViewModels;
using Microsoft.Extensions.Logging;
using Persistence.Neo4j.Repositories;

namespace Persistence.Neo4j.Workflows
{
    public class UserWorkflow
    {
        private readonly ILogger<UserWorkflow> _logger;
        private readonly IUserRepository _userRepository;

        public UserWorkflow(ILogger<UserWorkflow> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<object> CreateUser(Guid accountId, UserViewModel user)
        {
            var userNode = user.ToNode();

            return await _userRepository.Create(accountId, userNode);
        }
    }
}
