using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    TechnicTypeId = table.Column<int>(type: "integer", nullable: false),
                    RoomNumber = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TechnicTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Equipment",
                columns: new[] { "Id", "AccountNumber", "Name", "RoomNumber", "TechnicTypeId" },
                values: new object[,]
                {
                    { 1, "PC001", "ПК офисный №1", 101, 1 },
                    { 2, "PC002", "ПК офисный №2", 102, 1 },
                    { 3, "MN001", "Монитор Dell 24", 101, 2 },
                    { 4, "NB001", "Ноутбук Lenovo", 205, 3 },
                    { 5, "PR001", "МФУ Kyocera", 301, 4 }
                });

            migrationBuilder.InsertData(
                table: "TechnicTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Системный блок" },
                    { 2, "Монитор" },
                    { 3, "Ноутбук" },
                    { 4, "Принтер/МФУ" },
                    { 5, "Сетевое оборудование" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_AccountNumber",
                table: "Equipment",
                column: "AccountNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "TechnicTypes");
        }
    }
}
