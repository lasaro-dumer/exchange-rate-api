using Microsoft.EntityFrameworkCore.Migrations;

namespace Lasaro.ExchangeRate.Data.Migrations
{
    public partial class CurrencyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ForeignCurrencyCode",
                table: "CurrencyExchangeTransaction",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    MonthlyTransactionLimit = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Currency",
                columns: new[] { "Code", "MonthlyTransactionLimit" },
                values: new object[] { "USD", 200 });

            migrationBuilder.InsertData(
                table: "Currency",
                columns: new[] { "Code", "MonthlyTransactionLimit" },
                values: new object[] { "BRL", 300 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.AlterColumn<string>(
                name: "ForeignCurrencyCode",
                table: "CurrencyExchangeTransaction",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(3)",
                oldMaxLength: 3);
        }
    }
}
