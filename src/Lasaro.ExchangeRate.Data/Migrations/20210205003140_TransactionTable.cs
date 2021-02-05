using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lasaro.ExchangeRate.Data.Migrations
{
    public partial class TransactionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrencyExchangeTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LocalCurrencyAmount = table.Column<double>(type: "float", nullable: false),
                    ForeignCurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForeignCurrencyAmount = table.Column<double>(type: "float", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyExchangeTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrencyExchangeTransaction_Rate_RateId",
                        column: x => x.RateId,
                        principalTable: "Rate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyExchangeTransaction_RateId",
                table: "CurrencyExchangeTransaction",
                column: "RateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyExchangeTransaction");
        }
    }
}
