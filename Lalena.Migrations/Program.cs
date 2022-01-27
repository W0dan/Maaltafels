using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentMigrator.Runner;
using Lalena.Migrations.Runner;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lalena.Migrations
{
    public class Program
    {
        private const string ConnectionStringName = Lalena_Migrations_Constants.ConnectionString;
        private static readonly Assembly MigrationsAssembly = typeof(Program).Assembly;

        public static readonly IConfiguration Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false)
            .AddUserSecrets<Program>()
            .Build();

        [Option("--confirm", Description = "Auto-confirm running the migrations")]
        public bool AutoConfirm { get; set; }

        [Option("--connectionString", Description = "The connection string to the database")]
        public string ConnectionString { get; set; } = null!;

        [Option("--tag")] public List<string> Tags { get; set; } = new();

        [Option("--mode")] public Modes? Mode { get; set; }

        [Option("--target")] public long? TargetVersion { get; set; }

        public static int Main(string[] args)
        {
            return CommandLineApplication.Execute<Program>(args);
        }

        public void OnExecute()
        {
            try
            {
                Console.WriteLine($"Migration runner for {Lalena_Migrations_Constants.ApplicationName}");
                Console.WriteLine("Assembly: " + MigrationsAssembly.FullName);

                var mode = GetMode();

                if (!TryGetConnectionString(out var connectionString))
                    throw new ArgumentException($"Could not find connection string named '{ConnectionStringName}'");

                var tags = GetTags();

                var targetVersion = GetTargetVersion(mode);

                if (!AutoConfirm && !Confirm())
                    return;

                using var serviceProvider = MigrationServices.CreateServices(MigrationsAssembly, connectionString, tags, Program.Configuration);
                var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

                if (mode == Modes.Up)
                {
                    if (targetVersion != null)
                        runner.MigrateUp(targetVersion.Value);
                    else
                        runner.MigrateUp();
                }
                else
                {
                    runner.MigrateDown(targetVersion!.Value);
                }
            }
            catch (Exception e)
            {
                WriteError(e.Message);
                throw;
            }
        }

        private Modes GetMode()
        {
            if (Mode != null)
                return Mode.Value;

            if (AutoConfirm)
                return Modes.Up;

            var inputMode = Prompt.GetString("Do you want to upgrade up or down?", "up");

            if (!Enum.TryParse<Modes>(inputMode, true, out var mode))
                throw new ArgumentException($"\"{inputMode}\" is not a valid mode.");

            return mode;
        }

        private long? GetTargetVersion(Modes mode)
        {
            if (TargetVersion != null)
                return TargetVersion;

            if (AutoConfirm)
            {
                if (mode == Modes.Down)
                    throw new ArgumentException("Cannot perform a silent down migration without a target version.");

                return null;
            }

            if (mode == Modes.Up)
            {
                var inputVersion = Prompt.GetString("To what version do you want to migrate to?");
                if (string.IsNullOrWhiteSpace(inputVersion))
                    return null;
                return long.Parse(inputVersion);
            }
            else
            {
                var inputVersion = Prompt.GetString("To what version do you want to migrate down to?");

                if (!long.TryParse(inputVersion, out var parsedVersion))
                    throw new ArgumentException("A target version is required for a down migration.");
                return parsedVersion;
            }
        }

        private IEnumerable<string> GetTags()
        {
            if (AutoConfirm) return Tags;

            var inputTags = Tags.Any() ? Tags : PromptTags();

            var tags = inputTags.Any() ? inputTags : new List<string> { "DEV" };

            Console.WriteLine("Current environment tags are used: {0}", string.Join(", ", Tags));

            return tags;

            static List<string> PromptTags()
            {
                var tags = Prompt.GetString(
                    "What environment do you want to execute too (for seeding purposes, seperate with comma), when empty DEV tags are being used:");
                return (tags ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
            }
        }

        private static bool Confirm()
        {
            return Prompt.GetYesNo("Do you want to run migrations?", false);
        }

        private bool TryGetConnectionString(out string connectionString)
        {
            if (Validate(ConnectionString))
            {
                connectionString = ConnectionString;
                Console.WriteLine("Connection string (from command line): " + OmitPassword(connectionString));
                return true;
            }

            var configConnectionString = Configuration.GetConnectionString(ConnectionStringName);
            if (Validate(configConnectionString))
            {
                connectionString = configConnectionString;
                Console.WriteLine("Connection string (from user secrets): " + OmitPassword(connectionString));
                return true;
            }

            WriteWarning("Connection string was not provided (command line, user secrets)");
            connectionString = null!;

            if (AutoConfirm) return false;

            var inputConnectionString = Prompt.GetString("Connection string:");
            if (!Validate(inputConnectionString)) return false;

            connectionString = inputConnectionString!;
            return true;

            static bool Validate(string? value)
            {
                return !string.IsNullOrWhiteSpace(value);
            }
        }

        private static string OmitPassword(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);

            if (!string.IsNullOrWhiteSpace(builder.Password)) builder.Password = "***omitted***";

            return builder.ToString();
        }

        private static void WriteWarning(string text)
        {
            WriteColor(ConsoleColor.Yellow, text);
        }

        private static void WriteError(string text)
        {
            Console.Error.WriteLine(text);
        }

        private static void WriteColor(ConsoleColor color, string text)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = currentColor;
        }
    }
}