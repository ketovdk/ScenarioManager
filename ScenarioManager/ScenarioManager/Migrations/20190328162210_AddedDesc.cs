using Microsoft.EntityFrameworkCore.Migrations;

namespace ScenarioManager.Migrations
{
    public partial class AddedDesc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "UserGroups",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "UserGroups");
        }
    }
}
