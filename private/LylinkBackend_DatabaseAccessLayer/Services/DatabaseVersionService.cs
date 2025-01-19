using LylinkBackend_DatabaseAccessLayer.Models;
using MySqlConnector;

namespace LylinkBackend_DatabaseAccessLayer.Services
{
    internal class DatabaseVersionService(LylinkdbContext context) : IDatabaseVersionService
    {
        public string? GetDatabaseVersion()
        {
            try
            {
                return context.DatabaseVersions
                .OrderByDescending(databaseVersion => databaseVersion.UpdatedOn)
                .First().Version;
            }
            catch (MySqlException)
            {
                return null;
            }
        }
    }
}
