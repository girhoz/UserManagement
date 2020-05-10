using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class addBootCampModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_T_BootCamp",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    BatchId = table.Column<int>(nullable: false),
                    ClassId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_T_BootCamp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_T_BootCamp_TB_M_Batch_BatchId",
                        column: x => x.BatchId,
                        principalTable: "TB_M_Batch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_T_BootCamp_TB_M_Class_ClassId",
                        column: x => x.ClassId,
                        principalTable: "TB_M_Class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_T_BootCamp_TB_T_UserDetails_UserId",
                        column: x => x.UserId,
                        principalTable: "TB_T_UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_T_BootCamp_BatchId",
                table: "TB_T_BootCamp",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_T_BootCamp_ClassId",
                table: "TB_T_BootCamp",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_T_BootCamp_UserId",
                table: "TB_T_BootCamp",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_T_BootCamp");
        }
    }
}
