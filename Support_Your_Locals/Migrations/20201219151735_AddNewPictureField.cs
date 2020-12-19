using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Support_Your_Locals.Migrations
{
    public partial class AddNewPictureField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Business");

            migrationBuilder.AddColumn<byte[]>(
                name: "PictureData",
                table: "Business",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureData",
                table: "Business");

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "Business",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
