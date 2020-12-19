using Microsoft.EntityFrameworkCore.Migrations;

namespace Authentication.Persistance.Migrations
{
    public partial class Change_UserSchema_Column_Names : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrackModelSchema",
                table: "UserSchemas",
                newName: "TrackModel");

            migrationBuilder.RenameColumn(
                name: "DataModelSchema",
                table: "UserSchemas",
                newName: "DataModel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrackModel",
                table: "UserSchemas",
                newName: "TrackModelSchema");

            migrationBuilder.RenameColumn(
                name: "DataModel",
                table: "UserSchemas",
                newName: "DataModelSchema");
        }
    }
}
