namespace Common.Nodes
{
    public class UserNode : IEntity
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public bool IsDeleted { get; set; }

        public string GivenName { get; set; }
        public string FamilyName { get; set; }
    }
}
