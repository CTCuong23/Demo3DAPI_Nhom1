using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo3DAPI.Migrations
{
    /// <inheritdoc />
    public partial class ct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PlayerAccounts",
                keyColumn: "ID",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$FUCJqut.lIPbCZMqVjLNTuV1iEJAbQPvfUlNyASoJ.0Un1seDnGkq");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PlayerAccounts",
                keyColumn: "ID",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$FR/oNUWuBDX6QchH0gnEV.CkcY37UBjhQ1uZFNf0DTBGdSMQvK2C2");
        }
    }
}
