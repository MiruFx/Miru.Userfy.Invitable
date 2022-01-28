using FluentMigrator;
using Miru.Databases.Migrations.FluentMigrator;

namespace AltBank.Database.Migrations;
 
[Migration(202112281752)]
public class AlterUsersAddInvitation : Migration
{
    public override void Up()
    {
        Alter.Table("Users")
            .AddColumn("InvitationToken").AsString(128).Nullable().Indexed()
            .AddColumn("InvitationAcceptedAt").AsDateTime().Nullable()
            .AddColumn("IsInactive").AsBoolean().Nullable().Indexed();
    }

    public override void Down()
    {
        Delete.Column("InvitationToken").FromTable("Users");
        Delete.Column("InvitationAcceptedAt").FromTable("Users");
        Delete.Column("IsInactive").FromTable("Users");
    }
}
