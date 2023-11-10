using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetCore.Migrations
{
    public partial class newchange1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Tire",
                table: "Trucks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Tire",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tire",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "Tire",
                table: "Cars");
        }
    }
}
