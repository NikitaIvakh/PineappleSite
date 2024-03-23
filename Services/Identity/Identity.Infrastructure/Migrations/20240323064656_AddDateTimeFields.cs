using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDateTimeFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedTime",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "ModifiedTime", "PasswordHash", "SecurityStamp" },
                values: new object[] { "90fecd3a-48a9-4f35-b208-56bb546c050e", null, null, "AQAAAAIAAYagAAAAEJxoqjGdpxMKBiS0W8SFW0iCj7GJbGU19VHTE+188TWyVQrcFyN5PFMtcv4TnzQScg==", "2d790ca0-d12b-401c-9dbe-0d267071b804" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9e224968-33e4-4652-b7b7-8574d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "ModifiedTime", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3fe38d31-1800-42d6-8d18-de9ff0ff33da", null, null, "AQAAAAIAAYagAAAAEPNSGiImqWk9iiFZF1L8jZZq7aceWHWVLDlFnNH6NkVddSEofeO82YowAodmfoeB0w==", "f04faa3d-afda-4946-994f-4598b3ddc617" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ModifiedTime",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b6f19f47-d90a-4539-82fa-eb74d44122d5", "AQAAAAIAAYagAAAAENvP8lPow8c592I3p/JX5vYbaBSkqvcnV6DzVWwGp2UY48hgcfqKCTJ1s8yX9eWTeA==", "6fe30a45-c68b-4c7c-bfa8-7c01246b591f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9e224968-33e4-4652-b7b7-8574d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b53a8963-be89-4458-9b8a-21cf760e5057", "AQAAAAIAAYagAAAAEN96/+EZRFwam35lA5NRyyty770U1QteT9TQCESKeDLOs0LrLR1j/YFDTt5TYRkQyQ==", "654566b3-443f-4d7a-b177-5e8d0d30d50f" });
        }
    }
}
