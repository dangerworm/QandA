namespace Common
{
    public interface IEntity
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public bool IsDeleted { get; set; }
    }
}
