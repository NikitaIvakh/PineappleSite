using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldForImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Avatar",
                table: "AspNetUsers",
                type: "bytea",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "Avatar", "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { null, "aa23be28-48c4-48b4-989d-3042ac8a7b1d", "AQAAAAIAAYagAAAAECEOy8x7i2k/Rkrah08LdM/TZnNTGLWVSjXO6KC8IL5g61UtMkbvfs38uHcttPRBzA==", "4767712e-ded3-49c0-8a8d-c25b3b04e6c4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9e224968-33e4-4652-b7b7-8574d048cdb9",
                columns: new[] { "Avatar", "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { null, "f95ae16c-406f-46d4-a631-73375be1f011", "AQAAAAIAAYagAAAAEE2QzAa4iBbW3/udlJoOJ9ETF7o+qbgk4jgY++VKIHySC8DcGWe/F0zTqEgYevXmaA==", "aaa2ebeb-e001-4292-aed2-f46c8a7bd482" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a6e4e198-e752-4eb2-b889-89086d1fb6ed", "AQAAAAIAAYagAAAAEKB/AlgQMkbadYQ/f8rObKNProUAxTWxyoctS2GXx5c9iXdG054Y9UWVSOIC2vaBCw==", "353d6c5c-19cc-4e74-8043-0de282cae8e0" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9e224968-33e4-4652-b7b7-8574d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f27a4c76-95ab-4cd1-a093-992626f0c292", "AQAAAAIAAYagAAAAEJL1B72qhni4iDVlKuPJgs6ZCvbS/A6E90zyczt2kyDtFjridRt/rdb+PcnpihEg/Q==", "5eb3424f-6d70-4597-880c-67fcb4702890" });
        }
    }
}
