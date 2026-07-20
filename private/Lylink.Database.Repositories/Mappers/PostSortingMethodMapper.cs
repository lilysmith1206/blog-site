using Lylink.Database.Context.Models;

namespace Lylink.Database.Repositories.Mappers
{
    public static class PostSortingMethodMapper
    {
        public static Lylink.Shared.Models.PostSortingMethod Map(this PostSortingMethod databasePostSortingMethod)
        {
            bool successfulParse = Enum.TryParse(typeof(Lylink.Shared.Models.PostSortingMethod), databasePostSortingMethod.SortingName, out object? parsedSortingMethod);

            if (parsedSortingMethod is Lylink.Shared.Models.PostSortingMethod postSortingMethod)
            {
                return postSortingMethod;
            }

            throw new InvalidDataException($"Category has sorting method {databasePostSortingMethod.SortingName ?? "null sorting method"}, which is not supported by enum.");
        }
    }
}
