using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CardsAgainstHumanity.Models.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Deck",
                columns: table => new
                {
                    DeckID = table.Column<Guid>(nullable: false),
                    DeckName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deck", x => x.DeckID);
                });

            migrationBuilder.CreateTable(
                name: "Card",
                columns: table => new
                {
                    CardID = table.Column<Guid>(nullable: false),
                    CardText = table.Column<string>(nullable: false),
                    CategoryID = table.Column<int>(nullable: false),
                    PickAmount = table.Column<int>(nullable: false),
                    DeckID = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Card", x => x.CardID);
                    table.ForeignKey(
                        name: "FK_Card_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Card_Deck_DeckID",
                        column: x => x.DeckID,
                        principalTable: "Deck",
                        principalColumn: "DeckID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "CategoryID", "CategoryName" },
                values: new object[,]
                {
                    { 1, "White Cards" },
                    { 2, "Black Cards" },
                    { 3, "Blank Cards" }
                });

            migrationBuilder.InsertData(
                table: "Deck",
                columns: new[] { "DeckID", "DeckName" },
                values: new object[] { new Guid("66c59f97-20ea-43b2-8393-40e2662f7176"), "Dutch Deck By Thijs" });

            migrationBuilder.CreateIndex(
                name: "IX_Card_CategoryID",
                table: "Card",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Card_DeckID",
                table: "Card",
                column: "DeckID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Card");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Deck");
        }
    }
}
