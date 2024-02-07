using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Favourite.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavouriteHeaders",
                columns: table => new
                {
                    FavouriteHeaderId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavouriteHeaders", x => x.FavouriteHeaderId);
                });

            migrationBuilder.CreateTable(
                name: "FavouriteDetails",
                columns: table => new
                {
                    FavouriteDetailsId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FavouriteHeaderId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavouriteDetails", x => x.FavouriteDetailsId);
                    table.ForeignKey(
                        name: "FK_FavouriteDetails_FavouriteHeaders_FavouriteHeaderId",
                        column: x => x.FavouriteHeaderId,
                        principalTable: "FavouriteHeaders",
                        principalColumn: "FavouriteHeaderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteDetails_FavouriteHeaderId",
                table: "FavouriteDetails",
                column: "FavouriteHeaderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavouriteDetails");

            migrationBuilder.DropTable(
                name: "FavouriteHeaders");
        }
    }
}
