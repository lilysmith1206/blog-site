using Microsoft.Extensions.DependencyInjection;
using FluentMigrator.Runner;
using LylinkBackend_DatabaseAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace LylinkDatabase.MigrationScripts;

internal class Program
{
    private const string ConnectionString = "server=localhost;port=3306;database=test_lylinkdb;user=root;password=root";

    static void Main(string[] args)
    {
        using var serviceProvider = CreateServices();
        using var scope = serviceProvider.CreateScope();

        // Run migrations
        UpdateDatabase(scope.ServiceProvider);
    }

    private static ServiceProvider CreateServices()
    {
        return new ServiceCollection()
            .AddDbContext<LylinkdbContext>(options =>
            {
                options.UseMySql(ConnectionString, ServerVersion.Parse("11.5.2-mariadb"));
            })
            // Add common FluentMigrator services
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                // Add database support (choose your provider)
                .AddMySql()
                .WithGlobalConnectionString(ConnectionString)
                // Define the assembly containing the migrations
                .ScanIn(typeof(Program).Assembly).For.All())
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider(false);
    }

    private static void UpdateDatabase(IServiceProvider serviceProvider)
    {
        // Instantiate the runner
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        var versionLoader = serviceProvider.GetRequiredService<IVersionLoader>();

        // Load applied migrations
        versionLoader.LoadVersionInfo();

        long currentVersion = versionLoader.VersionInfo
            .AppliedMigrations()
            .DefaultIfEmpty(-1)
            .Max();

        try
        {
            runner.MigrateUp();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: {0}", ex);

            try
            {
                // Roll back to the last known good version
                runner.MigrateDown(currentVersion);
            }
            catch (Exception rollbackEx)
            {
                Console.WriteLine("Rollback failed:");
                Console.WriteLine(rollbackEx);
            }
        }
    }
}
