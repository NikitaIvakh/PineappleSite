using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4c7d01a4-ca4e-4212-b6b6-684db8a22c0b", "AQAAAAIAAYagAAAAEC1CJaFeay2qnVsmdjTaMMghn4cxisnCAIPV+B9fJuk3Jdlu3CNqB7lXcl8wGJ0RSA==", "70c40524-ca25-48a1-a50c-06e3674728e5" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9e224968-33e4-4652-b7b7-8574d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a75560c1-65a8-48a0-88e6-70188fe78d80", "AQAAAAIAAYagAAAAEAuVHLlFwvOk5mlcqzy4DT1k+6wTnCKS09Mep4yWX+aaP/LPEIQfKRNWEPYoGlckRA==", "4aa9d5a9-226b-4f08-bc72-3e0a33acc6aa" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "05f1feac-f374-448e-be76-8cb914456f02", "AQAAAAIAAYagAAAAEEYjifE5hbwWYwn4u4O95MV0bSXVLCjzYD8RWwrj0KSsAoPGamW4BgKXuiI9uvXQ7g==", "fa247f7e-505a-4845-b9a1-d1d2f453158a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9e224968-33e4-4652-b7b7-8574d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5e658894-78f5-4d53-a1b0-6ed60f96fa21", "AQAAAAIAAYagAAAAEE8RRetUm+3eXCPQpthLMBz1B80Uu52KiiKtxkiUOuCwNpXs4tVNsMT8m/Kf+W00lQ==", "55547e60-c44e-49b0-8ebe-b431dd0a343c" });
        }
    }
}
