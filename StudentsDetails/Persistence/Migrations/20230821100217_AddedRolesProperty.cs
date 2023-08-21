using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentsDetails.Persistence.Migrations
{
    public partial class AddedRolesProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                schema: "dbo",
                table: "UserModel",
                newName: "Roles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Roles",
                schema: "dbo",
                table: "UserModel",
                newName: "Role");
        }
    }
}
