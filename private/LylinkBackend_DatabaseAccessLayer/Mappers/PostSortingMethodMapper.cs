using LylinkBackend_DatabaseAccessLayer.Models;

namespace LylinkBackend_DatabaseAccessLayer.Mappers
{
    public static class PostSortingMethodMapper
    {
        public static LylinkShared.Models.PostSortingMethod Map(this PostSortingMethod databasePostSortingMethod)
        {
            bool successfulParse = Enum.TryParse(typeof(LylinkShared.Models.PostSortingMethod), databasePostSortingMethod.SortingName, out object? parsedSortingMethod);

            if (parsedSortingMethod is LylinkShared.Models.PostSortingMethod postSortingMethod)
            {
                return postSortingMethod;
            }

            throw new InvalidDataException($"Category has sorting method {databasePostSortingMethod.SortingName ?? "null sorting method"}, which is not supported by enum.");
        }
    }
}
