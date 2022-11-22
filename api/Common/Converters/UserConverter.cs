using Common.Nodes;
using Common.ViewModels;

namespace Common.Converters
{
    public static class UserConverter
    {
        public static UserNode ToNode(this UserViewModel input)
        {
            var output = ConverterBase.ToNode<UserNode>(input);

            output.GivenName = input.GivenName;
            output.FamilyName = input.FamilyName;

            return output;
        }

        public static UserViewModel ToViewModel(this UserNode input)
        {
            var output = ConverterBase.ToViewModel<UserViewModel>(input);

            output.GivenName = input.GivenName;
            output.FamilyName = input.FamilyName;

            return output;
        }
    }
}
