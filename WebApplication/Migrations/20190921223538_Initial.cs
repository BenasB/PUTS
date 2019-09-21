using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Problems",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    InputDescription = table.Column<string>(nullable: false),
                    OutputDescription = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Problems", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Test",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GivenInput = table.Column<string>(nullable: true),
                    ExpectedOutput = table.Column<string>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    ProblemID = table.Column<int>(nullable: true),
                    Explanation = table.Column<string>(nullable: true),
                    ProblemID1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Test_Problems_ProblemID1",
                        column: x => x.ProblemID1,
                        principalTable: "Problems",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Test_Problems_ProblemID",
                        column: x => x.ProblemID,
                        principalTable: "Problems",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Test_ProblemID1",
                table: "Test",
                column: "ProblemID1");

            migrationBuilder.CreateIndex(
                name: "IX_Test_ProblemID",
                table: "Test",
                column: "ProblemID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Test");

            migrationBuilder.DropTable(
                name: "Problems");
        }
    }
}
