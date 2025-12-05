using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo3DAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationProductCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "PlayerAccounts",
                keyColumn: "ID",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$1xjsNGmitfAGs.WH5PNdtuDiUXDXJYfiPDxrK/RDMOxIdMbyQuJQy");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "PlayerAccounts",
                keyColumn: "ID",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$obR2AsqZJ57GCE8CzO/4AOCLrQoXEHo.JRVv1IYWeJcEsfpXtkKcq");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "ID");
        }
    }
}
