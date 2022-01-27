using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lalena.Migrations.Runner
{
    public static class MigrationServices
    {
        public static ServiceProvider CreateServices(
            Assembly migrationsAssembly,
            string connectionString,
            IEnumerable<string> tags,
            IConfiguration configuration)
        {
            return new ServiceCollection()
                .ConfigureMigrationsRunner(migrationsAssembly, connectionString, tags, configuration, ConfigureSqlMigrationRunner)
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .Configure<LogFileFluentMigratorLoggerOptions>(opt =>
                {
                    opt.ShowSql = true;
                    opt.ShowElapsedTime = true;
                })
                .BuildServiceProvider(false);
        }

        public static IServiceCollection ConfigureMigrationsRunner(this IServiceCollection serviceCollection,
            Assembly migrationsAssembly,
            string connectionString,
            IEnumerable<string> tags,
            IConfiguration configuration,
            Action<IMigrationRunnerBuilder> configureMigrationRunner)
        {
            return serviceCollection
                .AddFluentMigratorCore()
                .ConfigureRunner(rb =>
                {
                    configureMigrationRunner(rb);
                    rb.WithGlobalConnectionString(connectionString);
                    rb.ScanIn(migrationsAssembly).For.All();
                })
                .Configure<RunnerOptions>(opt => { opt.Tags = tags.ToArray(); });
        }

        public static void ConfigureSqlMigrationRunner(IMigrationRunnerBuilder migrationRunnerBuilder) =>
            migrationRunnerBuilder.AddSqlServer();
    }
}