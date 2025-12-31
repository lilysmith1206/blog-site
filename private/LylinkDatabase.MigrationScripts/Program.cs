using Microsoft.Extensions.DependencyInjection;
using FluentMigrator.Runner;

namespace LylinkDatabase.MigrationScripts;

internal class Program
{
    static int Main(string[] args)
    {
        if (args.Length == 0 || string.IsNullOrWhiteSpace(args[0]))
        {
            Console.Error.WriteLine("Connection string must be provided as the first argument.");
            return 1;
        }

        var connectionString = args[0];

        Console.WriteLine("Connection String: {0}", connectionString);

        using var serviceProvider = CreateServices(connectionString);
        using var scope = serviceProvider.CreateScope();

        // Run migrations
        UpdateDatabase(scope.ServiceProvider);

        return 0;
    }

    private static ServiceProvider CreateServices(string connectionString)
    {
        return new ServiceCollection()
            // Add common FluentMigrator services
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                // Add database support (choose your provider)
                .AddMySql()
                .WithGlobalConnectionString(connectionString)
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
