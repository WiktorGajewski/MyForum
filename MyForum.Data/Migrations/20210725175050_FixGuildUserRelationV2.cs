using Microsoft.EntityFrameworkCore.Migrations;

namespace MyForum.Data.Migrations
{
    public partial class FixGuildUserRelationV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Guilds_ManagedGuildId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ManagedGuildId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ManagedGuildId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Invitations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GuildmasterId",
                table: "Guilds",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Guilds_GuildmasterId",
                table: "Guilds",
                column: "GuildmasterId",
                unique: true,
                filter: "[GuildmasterId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Guilds_AspNetUsers_GuildmasterId",
                table: "Guilds",
                column: "GuildmasterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guilds_AspNetUsers_GuildmasterId",
                table: "Guilds");

            migrationBuilder.DropIndex(
                name: "IX_Guilds_GuildmasterId",
                table: "Guilds");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Invitations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "GuildmasterId",
                table: "Guilds",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ManagedGuildId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ManagedGuildId",
                table: "AspNetUsers",
                column: "ManagedGuildId",
                unique: true,
                filter: "[ManagedGuildId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Guilds_ManagedGuildId",
                table: "AspNetUsers",
                column: "ManagedGuildId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
