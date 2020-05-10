using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class editBootCampModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TB_T_BootCamp",
                table: "TB_T_BootCamp");

            migrationBuilder.DropIndex(
                name: "IX_TB_T_BootCamp_UserId",
                table: "TB_T_BootCamp");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TB_T_BootCamp");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TB_T_BootCamp",
                table: "TB_T_BootCamp",
                columns: new[] { "UserId", "BatchId", "ClassId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TB_T_BootCamp",
                table: "TB_T_BootCamp");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TB_T_BootCamp",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TB_T_BootCamp",
                table: "TB_T_BootCamp",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TB_T_BootCamp_UserId",
                table: "TB_T_BootCamp",
                column: "UserId");
        }
    }
}
