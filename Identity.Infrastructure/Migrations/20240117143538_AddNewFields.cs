using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "Age", "ConcurrencyStamp", "Description", "PasswordHash", "SecurityStamp" },
                values: new object[] { 24, "63660acd-5e20-4665-b873-ce9533dca09f", "Test", "AQAAAAIAAYagAAAAEIZnktLIJUHOezNnD8bqwjEVN/IlNrqeqUzwT2q9gl/UmaRLH9vI9As+dA3Dq9fHPg==", "7c5bed72-1c6c-47f3-a634-49162d9a1151" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9e224968-33e4-4652-b7b7-8574d048cdb9",
                columns: new[] { "Age", "ConcurrencyStamp", "Description", "PasswordHash", "SecurityStamp" },
                values: new object[] { 24, "c76f4028-10f9-44ee-b6f9-bd049035c056", "Test", "AQAAAAIAAYagAAAAEEQ+sllYOyxfC8wYNwwtVhC47UTYXuv9BceQOWSLw06Q5D3/KZJJOUXPAVlQtV1v6Q==", "e9bce60e-2b20-4d35-9f59-fca54fdf45d9" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ab357e15-ff55-4bf3-9860-8e736670dcc2", "AQAAAAIAAYagAAAAEG8BwX93Tg507J7aIDIBV4tZQHvCLoTjFgVzKwSeMywii/HMH5e5tjS9YwOWrhibqg==", "0dfd21a8-c366-4d66-83f4-df7b50a4785a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9e224968-33e4-4652-b7b7-8574d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9aaada00-89da-4c69-aa9d-665903b65b7c", "AQAAAAIAAYagAAAAEIWK1fWNvNoAmeJo6J6okhoZtZkIkIIFiPwIo1MeBOYyWkYpgtcXBnKcwuwUdjLRVw==", "fd786608-589a-43b9-9a61-89e83b732bbc" });
        }
    }
}
