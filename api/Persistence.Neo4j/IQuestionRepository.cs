namespace Persistence.Neo4j
{
    public interface IQuestionRepository
    {
        Task<IList<IDictionary<string, object>>> SearchByText(string searchString);
    }
}