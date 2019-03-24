using Microsoft.EntityFrameworkCore.Migrations;

namespace ScenarioManager.Migrations
{
    public partial class multikey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_UserGroups_ParentGropId",
                table: "UserGroups");

            migrationBuilder.DropIndex(
                name: "IX_UserGroups_ParentGropId",
                table: "UserGroups");

            migrationBuilder.DropColumn(
                name: "ParentGropId",
                table: "UserGroups");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Controllers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ControllerScnarios",
                columns: table => new
                {
                    ScenarioId = table.Column<long>(nullable: false),
                    ControllerId = table.Column<long>(nullable: false),
                    TurnedOn = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControllerScnarios", x => new { x.ControllerId, x.ScenarioId });
                    table.ForeignKey(
                        name: "FK_ControllerScnarios_Controllers_ControllerId",
                        column: x => x.ControllerId,
                        principalTable: "Controllers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ControllerScnarios_Scenarios_ScenarioId",
                        column: x => x.ScenarioId,
                        principalTable: "Scenarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_ParentGroupId",
                table: "UserGroups",
                column: "ParentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ControllerScnarios_ScenarioId",
                table: "ControllerScnarios",
                column: "ScenarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_UserGroups_ParentGroupId",
                table: "UserGroups",
                column: "ParentGroupId",
                principalTable: "UserGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_UserGroups_ParentGroupId",
                table: "UserGroups");

            migrationBuilder.DropTable(
                name: "ControllerScnarios");

            migrationBuilder.DropIndex(
                name: "IX_UserGroups_ParentGroupId",
                table: "UserGroups");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Controllers");

            migrationBuilder.AddColumn<long>(
                name: "ParentGropId",
                table: "UserGroups",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_ParentGropId",
                table: "UserGroups",
                column: "ParentGropId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_UserGroups_ParentGropId",
                table: "UserGroups",
                column: "ParentGropId",
                principalTable: "UserGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
