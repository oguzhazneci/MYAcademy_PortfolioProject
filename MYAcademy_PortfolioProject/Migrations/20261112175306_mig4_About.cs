using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MYAcademy_PortfolioProject.Migrations
{
    /// <inheritdoc />
    public partial class mig4_About : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abouts",
                columns: table => new
                {
                    AboutId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Heading = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CvUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StatValue1 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StatLabel1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatValue2 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StatLabel2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatValue3 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StatLabel3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abouts", x => x.AboutId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Abouts");
        }
    }
}
