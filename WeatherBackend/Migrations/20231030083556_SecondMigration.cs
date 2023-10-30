using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherBackend.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeatherForecasts_Cities_CityId",
                table: "WeatherForecasts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WeatherForecasts",
                table: "WeatherForecasts");

            migrationBuilder.RenameTable(
                name: "WeatherForecasts",
                newName: "Forecasts");

            migrationBuilder.RenameIndex(
                name: "IX_WeatherForecasts_CityId",
                table: "Forecasts",
                newName: "IX_Forecasts_CityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Forecasts",
                table: "Forecasts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Forecasts_Cities_CityId",
                table: "Forecasts",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forecasts_Cities_CityId",
                table: "Forecasts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Forecasts",
                table: "Forecasts");

            migrationBuilder.RenameTable(
                name: "Forecasts",
                newName: "WeatherForecasts");

            migrationBuilder.RenameIndex(
                name: "IX_Forecasts_CityId",
                table: "WeatherForecasts",
                newName: "IX_WeatherForecasts_CityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WeatherForecasts",
                table: "WeatherForecasts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WeatherForecasts_Cities_CityId",
                table: "WeatherForecasts",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");
        }
    }
}
