namespace Common.ViewModels
{
    public class QuestionViewModel : IEntity
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public bool IsDeleted { get; set; }
        public string QuestionText { get; set; }
    }
}