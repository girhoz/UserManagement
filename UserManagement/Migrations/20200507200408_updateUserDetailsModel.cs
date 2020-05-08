using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class updateUserDetailsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DistrictId",
                table: "TB_T_UserDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StateId",
                table: "TB_T_UserDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ZipcodeId",
                table: "TB_T_UserDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TB_T_UserDetails_DistrictId",
                table: "TB_T_UserDetails",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_T_UserDetails_StateId",
                table: "TB_T_UserDetails",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_T_UserDetails_ZipcodeId",
                table: "TB_T_UserDetails",
                column: "ZipcodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TB_T_UserDetails_TB_M_District_DistrictId",
                table: "TB_T_UserDetails",
                column: "DistrictId",
                principalTable: "TB_M_District",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TB_T_UserDetails_TB_M_State_StateId",
                table: "TB_T_UserDetails",
                column: "StateId",
                principalTable: "TB_M_State",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TB_T_UserDetails_TB_M_Zipcode_ZipcodeId",
                table: "TB_T_UserDetails",
                column: "ZipcodeId",
                principalTable: "TB_M_Zipcode",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TB_T_UserDetails_TB_M_District_DistrictId",
                table: "TB_T_UserDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TB_T_UserDetails_TB_M_State_StateId",
                table: "TB_T_UserDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TB_T_UserDetails_TB_M_Zipcode_ZipcodeId",
                table: "TB_T_UserDetails");

            migrationBuilder.DropIndex(
                name: "IX_TB_T_UserDetails_DistrictId",
                table: "TB_T_UserDetails");

            migrationBuilder.DropIndex(
                name: "IX_TB_T_UserDetails_StateId",
                table: "TB_T_UserDetails");

            migrationBuilder.DropIndex(
                name: "IX_TB_T_UserDetails_ZipcodeId",
                table: "TB_T_UserDetails");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "TB_T_UserDetails");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "TB_T_UserDetails");

            migrationBuilder.DropColumn(
                name: "ZipcodeId",
                table: "TB_T_UserDetails");
        }
    }
}
