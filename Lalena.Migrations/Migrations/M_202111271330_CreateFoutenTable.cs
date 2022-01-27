using FluentMigrator;

namespace Lalena.Migrations.Migrations
{
    [Migration(202111271330)]
    public class M_202111271330_CreateFoutenTable : ForwardOnlyMigration
    {
        public override void Up()
        {
            Create.Table("Fouten")
                .WithColumn("Id").AsInt32().Identity().NotNullable().PrimaryKey()
                .WithColumn("Opgave").AsString().NotNullable()
                .WithColumn("CorrectAntwoord").AsInt32()
                .WithColumn("IngevuldAntwoord").AsInt32()
                .WithColumn("ResultaatId").AsGuid().ForeignKey(M_202111271325_CreateResultatenTable.ResultatenTableName, "Id");
        }
    }
}