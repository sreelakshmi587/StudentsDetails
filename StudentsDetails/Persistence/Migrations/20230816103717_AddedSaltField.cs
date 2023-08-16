using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentsDetails.Persistence.Migrations
{
    public partial class AddedSaltField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                schema: "dbo",
                table: "UserModel",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Salt",
                schema: "dbo",
                table: "UserModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserModel",
                schema: "dbo",
                table: "UserModel",
                column: "UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserModel",
                schema: "dbo",
                table: "UserModel");

            migrationBuilder.DropColumn(
                name: "Salt",
                schema: "dbo",
                table: "UserModel");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                schema: "dbo",
                table: "UserModel",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
