using FluentMigrator;

namespace Lalena.Migrations.Migrations
{
    [Migration(202111271325)]
    public class M_202111271325_CreateResultatenTable : ForwardOnlyMigration
    {
        public const string ResultatenTableName = "Resultaten";

        public override void Up()
        {
            Create.Table(ResultatenTableName)
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("DoorWie").AsString().NotNullable()
                .WithColumn("Wanneer").AsDateTime2().NotNullable()
                .WithColumn("Tafels").AsString().NotNullable()
                .WithColumn("Bewerkingen").AsString().NotNullable()
                .WithColumn("Punten").AsInt32().NotNullable()
                .WithColumn("MaxTeBehalen").AsInt32().NotNullable();
        }
    }
}
