using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class addUserRolesModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_T_UserRoles",
                columns: table => new
                {
                    User_Id = table.Column<int>(nullable: false),
                    Role_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_T_UserRoles", x => new { x.User_Id, x.Role_Id });
                    table.ForeignKey(
                        name: "FK_TB_T_UserRoles_TB_M_Role_Role_Id",
                        column: x => x.Role_Id,
                        principalTable: "TB_M_Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_T_UserRoles_TB_M_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "TB_M_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_T_UserRoles_Role_Id",
                table: "TB_T_UserRoles",
                column: "Role_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_T_UserRoles");
        }
    }
}
