using Microsoft.Extensions.Logging;
using Persistence.Neo4j.Entities;
using static Persistence.Neo4j.Constants;

namespace Persistence.Neo4j
{
    public class QuestionRepository : IQuestionRepository
    {
        INeo4jDataAccess _neo4jDataAccess;
        private ILogger<QuestionRepository> _logger;

        public QuestionRepository(INeo4jDataAccess neo4jDataAccess, ILogger<QuestionRepository> logger)
        {
            _neo4jDataAccess = neo4jDataAccess;
            _logger = logger;
        }

        public async Task<bool> Create(Question question)
        {
            if (string.IsNullOrWhiteSpace(question?.QuestionText))
            {
                throw new ArgumentNullException(nameof(question), "Question has no text");
            }

            var query =
                $"MERGE (q: {QUESTION} {{ questionText: ${nameof(question.QuestionText)} }}" +
                $"ON CREATE SET " +
                $"  q.created = '{DateTime.UtcNow:u}'," +
                $"  q.isDeleted = ${question.IsDeleted} " +
                $"RETURN true";

            IDictionary<string, object> parameters = new Dictionary<string, object>
            {
                { nameof(question.QuestionText), question.QuestionText },
                { nameof(question.IsDeleted), question.IsDeleted }
            };

            return await _neo4jDataAccess.ExecuteWriteTransactionAsync<bool>(query, parameters);
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
