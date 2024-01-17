using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateType : Migration
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

            migrationBuilder.AlterColumn<int>(
                name: "Age",
                table: "AspNetUsers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

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

            migrationBuilder.AlterColumn<int>(
                name: "Age",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "63660acd-5e20-4665-b873-ce9533dca09f", "AQAAAAIAAYagAAAAEIZnktLIJUHOezNnD8bqwjEVN/IlNrqeqUzwT2q9gl/UmaRLH9vI9As+dA3Dq9fHPg==", "7c5bed72-1c6c-47f3-a634-49162d9a1151" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9e224968-33e4-4652-b7b7-8574d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c76f4028-10f9-44ee-b6f9-bd049035c056", "AQAAAAIAAYagAAAAEEQ+sllYOyxfC8wYNwwtVhC47UTYXuv9BceQOWSLw06Q5D3/KZJJOUXPAVlQtV1v6Q==", "e9bce60e-2b20-4d35-9f59-fca54fdf45d9" });
        }
    }
}
