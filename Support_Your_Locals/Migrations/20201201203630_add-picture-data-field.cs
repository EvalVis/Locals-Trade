using Microsoft.EntityFrameworkCore.Migrations;

namespace Support_Your_Locals.Migrations
{
    public partial class addpicturedatafield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Business");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "Business",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
