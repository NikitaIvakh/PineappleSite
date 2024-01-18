using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFieldsForImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ImageLocalPath",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "ImageLocalPath", "ImageUrl", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7b91a4bd-aac6-4f83-9fca-ed88b9a82001", null, null, "AQAAAAIAAYagAAAAELHv6WdinEO47mxH4WyOyqrHO2UQxNB5g3565NEFrWyffzBoyh7GwAG+6AF40cfLsA==", "4ae1f425-e302-42a7-aefe-63eced3fa3c3" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9e224968-33e4-4652-b7b7-8574d048cdb9",
                columns: new[] { "ConcurrencyStamp", "ImageLocalPath", "ImageUrl", "PasswordHash", "SecurityStamp" },
                values: new object[] { "26db93b4-1df4-4c6d-a268-fff0cf91304e", null, null, "AQAAAAIAAYagAAAAECEexbe5Y83imPzqK8LDBPiFXYuXGmUhhuIyDUUueTnUj4UyGWOxqiHrJ/1tXpTU7g==", "c20c7a6f-d517-4ba2-8b00-cb94e3848027" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageLocalPath",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "AspNetUsers");

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
    }
}
