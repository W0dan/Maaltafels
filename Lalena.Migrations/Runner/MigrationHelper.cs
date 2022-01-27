namespace Lalena.Migrations.Runner
{
    public class MigrationHelper
    {
        public static string CreateForeignKeyName(string sourceSchema, string sourceTable, string targetSchema, string targetTable) =>
            $"FK_{sourceSchema}_{sourceTable}_{targetSchema}_{targetTable}";
    }
}