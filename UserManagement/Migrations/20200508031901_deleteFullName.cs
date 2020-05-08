using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class deleteFullName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "TB_T_UserDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "TB_T_UserDetails",
                nullable: true);
        }
    }
}
