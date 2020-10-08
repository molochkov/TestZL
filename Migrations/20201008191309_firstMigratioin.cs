using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestZL.Migrations
{
    public partial class firstMigratioin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocationMarkups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Syn = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationMarkups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoadItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    DateCreation = table.Column<DateTime>(nullable: false),
                    IsContainsCyrillic = table.Column<bool>(nullable: false),
                    IsContainsLatin = table.Column<bool>(nullable: false),
                    IsContainsDigits = table.Column<bool>(nullable: false),
                    IsContainsSpecialChars = table.Column<bool>(nullable: false),
                    IsCaseSensitivity = table.Column<bool>(nullable: false),
                    MarkupId = table.Column<Guid>(nullable: true),
                    LocalPathToFile = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoadItems_LocationMarkups_MarkupId",
                        column: x => x.MarkupId,
                        principalTable: "LocationMarkups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "LocationMarkups",
                columns: new[] { "Id", "Name", "Syn" },
                values: new object[] { new Guid("9139e3a1-99bb-4a41-b780-64351e4dbd03"), "отсутствует", "None" });

            migrationBuilder.InsertData(
                table: "LocationMarkups",
                columns: new[] { "Id", "Name", "Syn" },
                values: new object[] { new Guid("2ce9b57e-0cca-4814-afcb-a3bb1315c8b7"), "в именах файлов", "InFileName" });

            migrationBuilder.InsertData(
                table: "LocationMarkups",
                columns: new[] { "Id", "Name", "Syn" },
                values: new object[] { new Guid("7de3638f-a6b3-4cbd-9493-8cb2d632858e"), "в отдельном файле", "InSeparateFile" });

            migrationBuilder.CreateIndex(
                name: "IX_LoadItems_MarkupId",
                table: "LoadItems",
                column: "MarkupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoadItems");

            migrationBuilder.DropTable(
                name: "LocationMarkups");
        }
    }
}
