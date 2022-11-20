namespace Persistence.Neo4j.Entities
{
    public record Question
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public bool IsDeleted { get; set; }
        public string QuestionText { get; set; }
    }
}
