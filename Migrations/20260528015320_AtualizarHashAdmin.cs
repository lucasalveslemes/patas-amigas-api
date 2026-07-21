using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatasAmigas.API.Migrations
{
    /// <inheritdoc />
    public partial class AtualizarHashAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                column: "SenhaHash",
                value: "$2a$11$uU0/hMqDR/cgBtH7sB7.Xu3BPLUpNtZ1YBvFD3bvX8WFkhLCsJ/pC");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                column: "SenhaHash",
                value: "$2a$11$8K1p/a0dclxKMtHMHAGXEOmMGMFEnFCiRYRDkDpg4IqPkJG3bqTKW");
        }
    }
}
