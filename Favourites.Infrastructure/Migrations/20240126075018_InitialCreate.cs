using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Favourites.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavouritesHeaders",
                columns: table => new
                {
                    FavouritesHeaderId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavouritesHeaders", x => x.FavouritesHeaderId);
                });

            migrationBuilder.CreateTable(
                name: "FavouritesDetails",
                columns: table => new
                {
                    FavouritesDetailsId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FavouritesHeaderId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavouritesDetails", x => x.FavouritesDetailsId);
                    table.ForeignKey(
                        name: "FK_FavouritesDetails_FavouritesHeaders_FavouritesHeaderId",
                        column: x => x.FavouritesHeaderId,
                        principalTable: "FavouritesHeaders",
                        principalColumn: "FavouritesHeaderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavouritesDetails_FavouritesHeaderId",
                table: "FavouritesDetails",
                column: "FavouritesHeaderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavouritesDetails");

            migrationBuilder.DropTable(
                name: "FavouritesHeaders");
        }
    }
}
