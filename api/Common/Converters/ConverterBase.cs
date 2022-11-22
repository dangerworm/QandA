namespace Common.Converters
{
    public static class ConverterBase
    {
        public static T ToNode<T>(this IEntity viewModel)
            where T: IEntity, new()
        {
            return new T
            {
                Id = viewModel.Id,
                Created = viewModel.Created,
                IsDeleted = viewModel.IsDeleted
            };
        }

        public static T ToViewModel<T>(this IEntity node)
            where T: IEntity, new()
        {
            return new T
            {
                Id = node.Id,
                Created = node.Created,
                IsDeleted = node.IsDeleted
            };
        }
    }
}
