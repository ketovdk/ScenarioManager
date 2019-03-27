using Microsoft.EntityFrameworkCore.Migrations;

namespace ScenarioManager.Migrations
{
    public partial class usergroupToSensorsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FIO",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Info",
                table: "Users",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ControllerId",
                table: "SmartThings",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "UserGroupId",
                table: "SmartThings",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "ControllerId",
                table: "Sensors",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "UserGroupId",
                table: "Sensors",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "FIO",
                table: "Admins",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Info",
                table: "Admins",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SmartThings_UserGroupId",
                table: "SmartThings",
                column: "UserGroupId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_UserGroupId",
                table: "Sensors",
                column: "UserGroupId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_UserGroups_UserGroupId",
                table: "Sensors",
                column: "UserGroupId",
                principalTable: "UserGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SmartThings_UserGroups_UserGroupId",
                table: "SmartThings",
                column: "UserGroupId",
                principalTable: "UserGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_UserGroups_UserGroupId",
                table: "Sensors");

            migrationBuilder.DropForeignKey(
                name: "FK_SmartThings_UserGroups_UserGroupId",
                table: "SmartThings");

            migrationBuilder.DropIndex(
                name: "IX_SmartThings_UserGroupId",
                table: "SmartThings");

            migrationBuilder.DropIndex(
                name: "IX_Sensors_UserGroupId",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "FIO",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Info",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserGroupId",
                table: "SmartThings");

            migrationBuilder.DropColumn(
                name: "UserGroupId",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "FIO",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "Info",
                table: "Admins");

            migrationBuilder.AlterColumn<long>(
                name: "ControllerId",
                table: "SmartThings",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ControllerId",
                table: "Sensors",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);
        }
    }
}
