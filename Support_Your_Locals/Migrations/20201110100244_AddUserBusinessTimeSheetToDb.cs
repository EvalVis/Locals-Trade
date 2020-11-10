using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Support_Your_Locals.Migrations
{
    public partial class AddUserBusinessTimeSheetToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Passhash = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Business",
                columns: table => new
                {
                    BusinessID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<long>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Longitude = table.Column<string>(nullable: true),
                    Latitude = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Header = table.Column<string>(nullable: true),
                    Pictures = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Business", x => x.BusinessID);
                    table.ForeignKey(
                        name: "FK_Business_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    PricePerUnit = table.Column<decimal>(nullable: false),
                    Unit = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    BusinessID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Products_Business_BusinessID",
                        column: x => x.BusinessID,
                        principalTable: "Business",
                        principalColumn: "BusinessID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimeSheets",
                columns: table => new
                {
                    TimeSheetID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    From = table.Column<DateTime>(nullable: false),
                    To = table.Column<DateTime>(nullable: false),
                    Weekday = table.Column<int>(nullable: false),
                    BusinessID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSheets", x => x.TimeSheetID);
                    table.ForeignKey(
                        name: "FK_TimeSheets_Business_BusinessID",
                        column: x => x.BusinessID,
                        principalTable: "Business",
                        principalColumn: "BusinessID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Business_UserID",
                table: "Business",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BusinessID",
                table: "Products",
                column: "BusinessID");

            migrationBuilder.CreateIndex(
                name: "IX_TimeSheets_BusinessID",
                table: "TimeSheets",
                column: "BusinessID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "TimeSheets");

            migrationBuilder.DropTable(
                name: "Business");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
