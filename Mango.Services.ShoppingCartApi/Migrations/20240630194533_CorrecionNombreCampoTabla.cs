using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Services.ShoppingCartApi.Migrations
{
    /// <inheritdoc />
    public partial class CorrecionNombreCampoTabla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CarHeaderId",
                table: "CartHeaders",
                newName: "CartHeaderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CartHeaderId",
                table: "CartHeaders",
                newName: "CarHeaderId");
        }
    }
}
