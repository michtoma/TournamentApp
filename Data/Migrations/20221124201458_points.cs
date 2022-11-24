using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mundial.Data.Migrations
{
    public partial class points : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BetPoints",
                table: "Bettings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BetPoints",
                table: "Bettings");
        }
    }
}
