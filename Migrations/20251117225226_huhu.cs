using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Demo3DAPI.Migrations
{
    /// <inheritdoc />
    public partial class huhu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CategoryDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Dis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "PlayerAccounts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RoleID = table.Column<int>(type: "int", nullable: false, defaultValue: 2)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerAccounts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PlayerAccounts_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlayerAccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bills_PlayerAccounts_PlayerAccountId",
                        column: x => x.PlayerAccountId,
                        principalTable: "PlayerAccounts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerCharacters",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    PlayerAccountID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerCharacters", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PlayerCharacters_PlayerAccounts_PlayerAccountID",
                        column: x => x.PlayerAccountID,
                        principalTable: "PlayerAccounts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "User" }
                });

            migrationBuilder.InsertData(
                table: "PlayerAccounts",
                columns: new[] { "ID", "FullName", "Password", "PhoneNumber", "RoleID", "UserName" },
                values: new object[] { 1, "Admin", "$2a$11$obR2AsqZJ57GCE8CzO/4AOCLrQoXEHo.JRVv1IYWeJcEsfpXtkKcq", null, 1, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Bills_PlayerAccountId",
                table: "Bills",
                column: "PlayerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerAccounts_RoleID",
                table: "PlayerAccounts",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerCharacters_PlayerAccountID",
                table: "PlayerCharacters",
                column: "PlayerAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryID",
                table: "Products",
                column: "CategoryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "PlayerCharacters");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "PlayerAccounts");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
