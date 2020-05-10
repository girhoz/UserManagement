using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class modelRevision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TB_T_UserDetails_TB_M_Batch_BatchId",
                table: "TB_T_UserDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TB_T_UserDetails_TB_M_Class_ClassId",
                table: "TB_T_UserDetails");

            migrationBuilder.DropIndex(
                name: "IX_TB_T_UserDetails_BatchId",
                table: "TB_T_UserDetails");

            migrationBuilder.DropIndex(
                name: "IX_TB_T_UserDetails_ClassId",
                table: "TB_T_UserDetails");

            migrationBuilder.DropColumn(
                name: "BatchId",
                table: "TB_T_UserDetails");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "TB_T_UserDetails");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "TB_M_User");

            migrationBuilder.AddColumn<bool>(
                name: "LockStatus",
                table: "TB_M_User",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LockStatus",
                table: "TB_M_User");

            migrationBuilder.AddColumn<int>(
                name: "BatchId",
                table: "TB_T_UserDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "TB_T_UserDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockoutEnd",
                table: "TB_M_User",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_T_UserDetails_BatchId",
                table: "TB_T_UserDetails",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_T_UserDetails_ClassId",
                table: "TB_T_UserDetails",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_TB_T_UserDetails_TB_M_Batch_BatchId",
                table: "TB_T_UserDetails",
                column: "BatchId",
                principalTable: "TB_M_Batch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TB_T_UserDetails_TB_M_Class_ClassId",
                table: "TB_T_UserDetails",
                column: "ClassId",
                principalTable: "TB_M_Class",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
