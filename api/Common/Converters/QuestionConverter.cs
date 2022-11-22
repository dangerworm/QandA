using Common.Nodes;
using Common.ViewModels;

namespace Common.Converters
{
    public static class QuestionConverter
    {
        public static QuestionNode ToNode(this QuestionViewModel input)
        {
            var output = ConverterBase.ToNode<QuestionNode>(input);

            output.QuestionText = input.QuestionText;

            return output;
        }

        public static QuestionViewModel ToViewModel(this QuestionNode node)
        {
            var output = ConverterBase.ToViewModel<QuestionViewModel>(node);

            output.QuestionText = node.QuestionText;

            return output;
        }
    }
}
