using Microsoft.EntityFrameworkCore.Migrations;

namespace Bookweb.Migrations.Bookweb
{
    public partial class IsAcceptedUpdateForUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "FavoriteBooks",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "FavoriteBooks");
        }
    }
}
