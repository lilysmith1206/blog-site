using LylinkBackend_DatabaseAccessLayer.Models;
using LylinkBackend_DatabaseAccessLayer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace LylinkDatabase_MigrationScript
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            string? mariaDbConnection = config.GetSection("ConnectionStrings").GetValue<string>("MariaDbConnection")
                ?? throw new NullReferenceException("Can't retrieve maria db connection string.");

            LylinkdbContext context = new LylinkdbContext(mariaDbConnection);
            IDatabaseVersionService database = new DatabaseVersionService(context);

            bool successfulParse = int.TryParse(database.GetDatabaseVersion(), out int dbVersion);

            var updateScriptDetails = Directory.GetDirectories(Directory.GetCurrentDirectory())
                .Where(directory => new DirectoryInfo(directory).Name.Contains('_'))
                .Select(directory =>
                {
                    string[] directoryName = new DirectoryInfo(directory).Name.Split("_");

                    return new
                    {
                        Name = directoryName[1],
                        ScriptPath = $"{directory}/{directoryName[1]}.sql",
                        Version = directoryName[0]
                    };
                });

            if (successfulParse == false)
            {
                var databaseVersionUpdateScript = updateScriptDetails.Single(updateScript => updateScript.Version == "000");

                Console.WriteLine("No database version detected; running script add database version...");

                string sqlString = File.ReadAllText(databaseVersionUpdateScript.ScriptPath);

                var b = context.Database.ExecuteSqlRaw(sqlString);

                Console.WriteLine($"{databaseVersionUpdateScript.Name}: rows affected: {b}");

                updateScriptDetails = updateScriptDetails.Where(updateScript => updateScript.Version != databaseVersionUpdateScript.Version);

                dbVersion = int.Parse(database.GetDatabaseVersion() ?? throw new NullReferenceException("Can't get database version"));
            }

            updateScriptDetails = updateScriptDetails.Where(updateScript => int.TryParse(updateScript.Version, out int versionNum) && versionNum > dbVersion);

            int upgradeScriptCount = updateScriptDetails.Count();

            if (upgradeScriptCount == 0)
            {
                Console.WriteLine("Your database is up to date.");

                return;
            }

            Console.WriteLine($"Your database is {upgradeScriptCount} version{(upgradeScriptCount > 1 ? 's' : string.Empty)} behind.");
            Console.WriteLine("Press any key to upgrade the database.");

            _ = Console.ReadKey();

            foreach (var a in updateScriptDetails)
            {
                string sqlString = File.ReadAllText(a.ScriptPath);

                var b = context.Database.ExecuteSqlRaw(sqlString);

                Console.WriteLine($"{a.Name}: rows affected: {b}");
                Console.WriteLine("Press any key to continue upgrading.");

                _ = Console.ReadKey();
            }

            Debugger.Break();
        }
    }
}
