using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace living_room_api.Migrations
{
    public partial class newmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Computers",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Value = table.Column<decimal>(type: "decimal(16,2)", nullable: false),
                    isDesktop = table.Column<bool>(type: "bit", nullable: false),
                    isBeingSold = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Computers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HomeTheaters",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Value = table.Column<decimal>(type: "decimal(16,2)", nullable: false),
                    readsBlueRay = table.Column<bool>(type: "bit", nullable: false),
                    isBeingSold = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeTheaters", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CountryBirthLocation = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PeopleComputers",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PersonId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ComputerId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeopleComputers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PeopleHomeTheaters",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PersonId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    HomeTheaterId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeopleHomeTheaters", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PeopleTelevisions",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PersonId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TelevisionId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeopleTelevisions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Televisions",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Value = table.Column<decimal>(type: "decimal(16,2)", nullable: false),
                    is3D = table.Column<bool>(type: "bit", nullable: false),
                    isBeingSold = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Televisions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "People",
                columns: new[] { "ID", "BirthDate", "CountryBirthLocation", "Email", "FirstName", "LastName" },
                values: new object[] { "1234567890", null, "Brazil", "email@example.com", "blublu", "blabla" });

            migrationBuilder.InsertData(
                table: "Televisions",
                columns: new[] { "ID", "Brand", "CreationDate", "Model", "Value", "is3D", "isBeingSold" },
                values: new object[] { "1111111111", "Vony", null, "bleble", 0m, false, false });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "Email", "Name", "Password" },
                values: new object[] { "admin-1234", "admin@example.com", "admin", "V1ZkU2RHRlhOSGhOYWsw" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Computers");

            migrationBuilder.DropTable(
                name: "HomeTheaters");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "PeopleComputers");

            migrationBuilder.DropTable(
                name: "PeopleHomeTheaters");

            migrationBuilder.DropTable(
                name: "PeopleTelevisions");

            migrationBuilder.DropTable(
                name: "Televisions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
