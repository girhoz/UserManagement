using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class addUserDetailsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_T_UserDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    ReligionId = table.Column<int>(nullable: true),
                    BatchId = table.Column<int>(nullable: true),
                    ClassId = table.Column<int>(nullable: true),
                    WorkStatus = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_T_UserDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_T_UserDetails_TB_M_Batch_BatchId",
                        column: x => x.BatchId,
                        principalTable: "TB_M_Batch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_T_UserDetails_TB_M_Class_ClassId",
                        column: x => x.ClassId,
                        principalTable: "TB_M_Class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_T_UserDetails_TB_M_User_Id",
                        column: x => x.Id,
                        principalTable: "TB_M_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_T_UserDetails_TB_M_Religion_ReligionId",
                        column: x => x.ReligionId,
                        principalTable: "TB_M_Religion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_T_UserDetails_BatchId",
                table: "TB_T_UserDetails",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_T_UserDetails_ClassId",
                table: "TB_T_UserDetails",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_T_UserDetails_ReligionId",
                table: "TB_T_UserDetails",
                column: "ReligionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_T_UserDetails");
        }
    }
}
