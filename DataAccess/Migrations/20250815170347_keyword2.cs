using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class keyword2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KeywordBlogs_Keywords_KeywordId",
                table: "KeywordBlogs");

            migrationBuilder.AddForeignKey(
                name: "FK_KeywordBlogs_Keywords_KeywordId",
                table: "KeywordBlogs",
                column: "KeywordId",
                principalTable: "Keywords",
                principalColumn: "KeywordId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KeywordBlogs_Keywords_KeywordId",
                table: "KeywordBlogs");

            migrationBuilder.AddForeignKey(
                name: "FK_KeywordBlogs_Keywords_KeywordId",
                table: "KeywordBlogs",
                column: "KeywordId",
                principalTable: "Keywords",
                principalColumn: "KeywordId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
