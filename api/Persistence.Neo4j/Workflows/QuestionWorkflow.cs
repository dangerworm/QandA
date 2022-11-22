using Common.Converters;
using Common.ViewModels;
using Microsoft.Extensions.Logging;
using Persistence.Neo4j.Repositories;

namespace Persistence.Neo4j.Workflows
{
    public class QuestionWorkflow
    {
        private readonly ILogger<QuestionWorkflow> _logger;
        private readonly IQuestionRepository _questionRepository;

        public QuestionWorkflow(ILogger<QuestionWorkflow> logger, IQuestionRepository questionRepository)
        {
            _logger = logger;
            _questionRepository = questionRepository;
        }

        public async Task<object> CreateQuestion(UserViewModel user, QuestionViewModel question)
        {
            var userNode = user.ToNode();
            var questionNode = question.ToNode();

            return await _questionRepository.Create(userNode, questionNode);
        }
    }
}
