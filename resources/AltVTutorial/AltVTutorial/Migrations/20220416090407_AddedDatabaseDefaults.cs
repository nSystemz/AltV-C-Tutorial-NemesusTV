using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AltVTutorial.Migrations
{
    public partial class AddedDatabaseDefaults : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Franktion",
                table: "User",
                newName: "Fraktion");

            migrationBuilder.AlterColumn<float>(
                name: "PosZ",
                table: "User",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AlterColumn<float>(
                name: "PosY",
                table: "User",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AlterColumn<float>(
                name: "PosX",
                table: "User",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AlterColumn<float>(
                name: "PosR",
                table: "User",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Fraktion",
                table: "User",
                newName: "Franktion");

            migrationBuilder.AlterColumn<double>(
                name: "PosZ",
                table: "User",
                type: "double",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "PosY",
                table: "User",
                type: "double",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "PosX",
                table: "User",
                type: "double",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "PosR",
                table: "User",
                type: "double",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");
        }
    }
}
