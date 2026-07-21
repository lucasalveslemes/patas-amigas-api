using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatasAmigas.API.Migrations
{
    /// <inheritdoc />
    public partial class RenomearTabelaDoacaoParaDoacoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Doacao",
                table: "Doacao");

            migrationBuilder.RenameTable(
                name: "Doacao",
                newName: "Doacoes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Doacoes",
                table: "Doacoes",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Doacoes",
                table: "Doacoes");

            migrationBuilder.RenameTable(
                name: "Doacoes",
                newName: "Doacao");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Doacao",
                table: "Doacao",
                column: "Id");
        }
    }
}
