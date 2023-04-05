using System;
using Microsoft.EntityFrameworkCore.Migrations;
using USP_Project.Data.Models.Enums;

#nullable disable

namespace USP_Project.Data.Migrations
{
    public partial class AddinganEngineTypeandTransmissionenumsaswellasEngineSizepropertiestotheCarmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:engine_type", "petrol,diesel")
                .Annotation("Npgsql:Enum:transmission", "automatic,manual");

            migrationBuilder.AlterColumn<string[]>(
                name: "ImagePaths",
                table: "Cars",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0],
                oldClrType: typeof(string[]),
                oldType: "text[]",
                oldDefaultValue: new string[0]);

            migrationBuilder.AddColumn<EngineType>(
                name: "Engine",
                table: "Cars",
                type: "engine_type",
                nullable: false,
                defaultValue: EngineType.Petrol);

            migrationBuilder.AddColumn<decimal>(
                name: "EngineSize",
                table: "Cars",
                type: "numeric(10,8)",
                precision: 10,
                scale: 8,
                nullable: true);

            migrationBuilder.AddColumn<Transmission>(
                name: "Transmission",
                table: "Cars",
                type: "transmission",
                nullable: false,
                defaultValue: Transmission.Automatic);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Engine",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "EngineSize",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Transmission",
                table: "Cars");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:engine_type", "petrol,diesel")
                .OldAnnotation("Npgsql:Enum:transmission", "automatic,manual");

            migrationBuilder.AlterColumn<string[]>(
                name: "ImagePaths",
                table: "Cars",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0],
                oldClrType: typeof(string[]),
                oldType: "text[]",
                oldDefaultValue: new string[0]);
        }
    }
}
