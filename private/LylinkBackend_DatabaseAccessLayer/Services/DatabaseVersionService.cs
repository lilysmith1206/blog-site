using LylinkBackend_DatabaseAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace LylinkBackend_DatabaseAccessLayer.Services
{
    public class DatabaseVersionService(IDbContextFactory<LylinkdbContext> contextFactory) : IDatabaseVersionService
    {
        public string? GetDatabaseVersion()
        {
            try
            {
                using var context = contextFactory.CreateDbContext();

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
