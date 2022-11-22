namespace Common.ViewModels
{
    public class UserViewModel : IEntity
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public bool IsDeleted { get; set; }

        public string GivenName { get; set; }
        public string FamilyName { get; set; }

        public bool IsFollowedByCurrentUser { get; set; }
    }
}
