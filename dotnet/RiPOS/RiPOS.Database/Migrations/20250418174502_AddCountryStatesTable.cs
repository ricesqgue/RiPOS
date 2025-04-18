using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiPOS.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCountryStatesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CountryStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    ShortName = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryStates", x => x.Id);
                });
            
            // Insert data
            migrationBuilder.InsertData(
                table: "CountryStates",
                columns: ["Id", "Name", "ShortName"],
                values: new object[,]
                {
                    {1, "Aguascalientes", "AGU"},
                    {2, "Baja California", "BCN"},
                    {3, "Baja California Sur", "BCS"},
                    {4, "Campeche", "CAM"},
                    {5, "Chiapas", "CHP"},
                    {6, "Chihuahua", "CHH"},
                    {7, "Ciudad de México", "CMX"},
                    {8, "Coahuila", "COA"},
                    {9, "Colima", "COL"},
                    {10, "Durango", "DUR"},
                    {11, "Guanajuato", "GUA"},
                    {12, "Guerrero", "GRO"},
                    {13, "Hidalgo", "HID"},
                    {14, "Jalisco", "JAL"},
                    {15, "Estado de México", "MEX"},
                    {16, "Michoacán", "MIC"},
                    {17, "Morelos", "MOR"},
                    {18, "Nayarit", "NAY"},
                    {19, "Nuevo León", "NLE"},
                    {20, "Oaxaca", "OAX"},
                    {21, "Puebla", "PUE"},
                    {22, "Querétaro", "QUE"},
                    {23, "Quintana Roo", "ROO"},
                    {24, "San Luis Potosí", "SLP"},
                    {25, "Sinaloa", "SIN"},
                    {26, "Sonora", "SON"},
                    {27, "Tabasco", "TAB"},
                    {28, "Tamaulipas", "TAM"},
                    {29, "Tlaxcala", "TLA"},
                    {30, "Veracruz", "VER"},
                    {31, "Yucatán", "YUC"},
                    {32, "Zacatecas", "ZAC"}
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CountryStates");
        }
    }
}
