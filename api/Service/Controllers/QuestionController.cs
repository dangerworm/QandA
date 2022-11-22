using Common.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Persistence.Neo4j.Workflows;

namespace Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly ILogger<QuestionController> _logger;
        private readonly QuestionWorkflow _questionWorkflow;
        private readonly UserWorkflow _userWorkflow;

        public QuestionController(ILogger<QuestionController> logger, QuestionWorkflow questionWorkflow, UserWorkflow userWorkflow)
        {
            _logger = logger;
            _questionWorkflow = questionWorkflow;
            _userWorkflow = userWorkflow;
        }

        [HttpGet(Name = "CreateQuestion")]
        public async Task<object> Create()
        {
            var user = new UserViewModel { Id = Guid.NewGuid(), FamilyName = "Cunningham", GivenName = "Drew" };
            var question = new QuestionViewModel { QuestionText = "First question" };

            await _userWorkflow.CreateUser(new Guid("83f26d12-0a4f-460d-a001-4007514243e6"), user);
            return await _questionWorkflow.CreateQuestion(user, question);
        }
    }
}
