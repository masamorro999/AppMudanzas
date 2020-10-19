using Microsoft.EntityFrameworkCore.Migrations;

namespace TNSEjecutor.apiMudanza.Migrations
{
    public partial class InitalDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ejecutors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Document = table.Column<int>(nullable: false),
                    TransacDate = table.Column<string>(nullable: true),
                    NWorkTrips = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ejecutors", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Ejecutors",
                columns: new[] { "Id", "Document", "NWorkTrips", "TransacDate" },
                values: new object[] { 1, 1020441, "Case #1 = 2", "10/18/2020 6:52:53 PM" });

            migrationBuilder.InsertData(
                table: "Ejecutors",
                columns: new[] { "Id", "Document", "NWorkTrips", "TransacDate" },
                values: new object[] { 2, 43505, "Case #1 = 22", "10/18/2020 6:52:53 PM" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ejecutors");
        }
    }
}
