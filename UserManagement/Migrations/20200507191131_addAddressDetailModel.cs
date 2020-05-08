using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class addAddressDetailModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TB_T_UserDetails_TB_M_Batch_BatchId",
                table: "TB_T_UserDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TB_T_UserDetails_TB_M_Class_ClassId",
                table: "TB_T_UserDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TB_T_UserDetails_TB_M_Religion_ReligionId",
                table: "TB_T_UserDetails");

            migrationBuilder.AlterColumn<int>(
                name: "ReligionId",
                table: "TB_T_UserDetails",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClassId",
                table: "TB_T_UserDetails",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BatchId",
                table: "TB_T_UserDetails",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "TB_T_UserDetails",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TB_M_State",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_M_State", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_M_District",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    StateId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_M_District", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_M_District_TB_M_State_StateId",
                        column: x => x.StateId,
                        principalTable: "TB_M_State",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_M_Zipcode",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    DistrictId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_M_Zipcode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_M_Zipcode_TB_M_District_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "TB_M_District",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_M_District_StateId",
                table: "TB_M_District",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_M_Zipcode_DistrictId",
                table: "TB_M_Zipcode",
                column: "DistrictId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_TB_T_UserDetails_TB_M_Religion_ReligionId",
                table: "TB_T_UserDetails",
                column: "ReligionId",
                principalTable: "TB_M_Religion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TB_T_UserDetails_TB_M_Batch_BatchId",
                table: "TB_T_UserDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TB_T_UserDetails_TB_M_Class_ClassId",
                table: "TB_T_UserDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TB_T_UserDetails_TB_M_Religion_ReligionId",
                table: "TB_T_UserDetails");

            migrationBuilder.DropTable(
                name: "TB_M_Zipcode");

            migrationBuilder.DropTable(
                name: "TB_M_District");

            migrationBuilder.DropTable(
                name: "TB_M_State");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "TB_T_UserDetails");

            migrationBuilder.AlterColumn<int>(
                name: "ReligionId",
                table: "TB_T_UserDetails",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ClassId",
                table: "TB_T_UserDetails",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "BatchId",
                table: "TB_T_UserDetails",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_TB_T_UserDetails_TB_M_Batch_BatchId",
                table: "TB_T_UserDetails",
                column: "BatchId",
                principalTable: "TB_M_Batch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TB_T_UserDetails_TB_M_Class_ClassId",
                table: "TB_T_UserDetails",
                column: "ClassId",
                principalTable: "TB_M_Class",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TB_T_UserDetails_TB_M_Religion_ReligionId",
                table: "TB_T_UserDetails",
                column: "ReligionId",
                principalTable: "TB_M_Religion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
