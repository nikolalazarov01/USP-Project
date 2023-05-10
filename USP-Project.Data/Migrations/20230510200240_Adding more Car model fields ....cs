using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace USP_Project.Data.Migrations
{
    public partial class AddingmoreCarmodelfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:engine_type", "petrol,diesel")
                .Annotation("Npgsql:Enum:transmission", "automatic,manual")
                .Annotation("Npgsql:PostgresExtension:fuzzystrmatch", ",,")
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:engine_type", "petrol,diesel")
                .Annotation("Npgsql:Enum:transmission", "automatic,manual")
                .OldAnnotation("Npgsql:Enum:engine_type", "petrol,diesel")
                .OldAnnotation("Npgsql:Enum:transmission", "automatic,manual")
                .OldAnnotation("Npgsql:PostgresExtension:fuzzystrmatch", ",,");

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
