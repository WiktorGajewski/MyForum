using Microsoft.EntityFrameworkCore.Migrations;

namespace MyForum.Data.Migrations
{
    public partial class AddLikesFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_AspNetUsers_FromUserId",
                table: "Likes");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfLikes",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_AspNetUsers_FromUserId",
                table: "Likes",
                column: "FromUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_AspNetUsers_FromUserId",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "NumberOfLikes",
                table: "Messages");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_AspNetUsers_FromUserId",
                table: "Likes",
                column: "FromUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
