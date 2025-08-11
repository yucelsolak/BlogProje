using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class slugId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_BlogCategories_CategoryId",
                table: "Blogs");

            migrationBuilder.RenameColumn(
                name: "SludId",
                table: "Slugs",
                newName: "SlugId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_BlogCategories_CategoryId",
                table: "Blogs",
                column: "CategoryId",
                principalTable: "BlogCategories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_BlogCategories_CategoryId",
                table: "Blogs");

            migrationBuilder.RenameColumn(
                name: "SlugId",
                table: "Slugs",
                newName: "SludId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_BlogCategories_CategoryId",
                table: "Blogs",
                column: "CategoryId",
                principalTable: "BlogCategories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
