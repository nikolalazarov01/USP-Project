using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace USP_Project.Data.Migrations
{
    public partial class AddinganImagePathspropertytotheCarModelsupportedonlyinPostgres : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "ImagePaths",
                table: "Car",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePaths",
                table: "Car");
        }
    }
}
