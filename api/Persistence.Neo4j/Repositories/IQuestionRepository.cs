using Common.Nodes;

namespace Persistence.Neo4j.Repositories
{
    public interface IQuestionRepository
    {
        Task<object> Create(UserNode user, QuestionNode question);

        Task<IList<IDictionary<string, object>>> SearchByText(string searchString);
    }
}