using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mundial.Data.Migrations
{
    public partial class Initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoalDif",
                table: "Countries");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GoalDif",
                table: "Countries",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
