using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDateTimeValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c035a941-d015-4364-b16f-cba9cbedf86a", new DateTime(2024, 3, 23, 6, 48, 32, 244, DateTimeKind.Utc).AddTicks(2702), "AQAAAAIAAYagAAAAEP0JRe+5jVaqLCQgXrc+LFGMdWXNAhwh21U1NYIc9sUd5Ij9Z0RfB/AjC38rr/H63Q==", "023b865c-a0cb-4f48-ae57-e6e38438ff41" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9e224968-33e4-4652-b7b7-8574d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "PasswordHash", "SecurityStamp" },
                values: new object[] { "575be72e-2969-4362-a653-4e268cfd138b", new DateTime(2024, 3, 23, 6, 48, 32, 199, DateTimeKind.Utc).AddTicks(7122), "AQAAAAIAAYagAAAAEFyTRnFXqQksQPak07qhm9Ler0oO/0oagRISAzwflzlkEGGiDkd1RPzUvai3DoZ/AQ==", "0ec27a15-bddf-454b-969a-46bbfbba05b7" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "PasswordHash", "SecurityStamp" },
                values: new object[] { "90fecd3a-48a9-4f35-b208-56bb546c050e", null, "AQAAAAIAAYagAAAAEJxoqjGdpxMKBiS0W8SFW0iCj7GJbGU19VHTE+188TWyVQrcFyN5PFMtcv4TnzQScg==", "2d790ca0-d12b-401c-9dbe-0d267071b804" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9e224968-33e4-4652-b7b7-8574d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3fe38d31-1800-42d6-8d18-de9ff0ff33da", null, "AQAAAAIAAYagAAAAEPNSGiImqWk9iiFZF1L8jZZq7aceWHWVLDlFnNH6NkVddSEofeO82YowAodmfoeB0w==", "f04faa3d-afda-4946-994f-4598b3ddc617" });
        }
    }
}
