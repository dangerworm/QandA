using Common.Nodes;
using Microsoft.Extensions.Logging;
using static Persistence.Neo4j.NodeTypes;
using static Persistence.Neo4j.Relationships;

namespace Persistence.Neo4j.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private INeo4jDataAccess _neo4jDataAccess;
        private ILogger<QuestionRepository> _logger;

        public QuestionRepository(INeo4jDataAccess neo4jDataAccess, ILogger<QuestionRepository> logger)
        {
            _neo4jDataAccess = neo4jDataAccess;
            _logger = logger;
        }

        public async Task<object> Create(UserNode user, QuestionNode question)
        {
            // TODO: Move this to a validator
            if (string.IsNullOrWhiteSpace(question?.QuestionText))
            {
                throw new ArgumentNullException(nameof(question), "Question has no text");
            }

            var query =
                $"MATCH (user:{USER} {{ Id: '{user.Id}' }}) " +
                $"MATCH (question: {QUESTION} {{ QuestionText: ${nameof(question.QuestionText)} }}) " +
                $"MERGE (user)-[asked:{ASKED}]->(question) " +
                $"ON CREATE SET " +
                $"  asked.Created = '{DateTime.UtcNow:u}', " +
                $"  asked.IsDeleted = false, " +
                $"  question.Created = '{DateTime.UtcNow:u}', " +
                $"  question.IsDeleted = false " +
                $"RETURN user, asked, question";

            IDictionary<string, object> parameters = new Dictionary<string, object>
            {
                { nameof(question.QuestionText), question.QuestionText }
            };

            return await _neo4jDataAccess.ExecuteWriteTransactionAsync<object>(query, parameters);
        }

        public async Task<IList<IDictionary<string, object>>> SearchByText(string searchString)
        {
            var query =
                $"MATCH (q: {QUESTION}) " +
                $"WHERE toUpper(q.questionText) CONTAINS toUpper(${nameof(searchString)}) " +
                @"RETURN q{ id: q.id, questionText: q.questionText } " +
                $"ORDERBY q.questionText " +
                $"LIMIT 20";

            var parameters = new Dictionary<string, object> { { nameof(searchString), searchString } };
            var questions = await _neo4jDataAccess.ExecuteReadDictionaryAsync(query, "q", parameters);

            return questions;
        }
    }
}
