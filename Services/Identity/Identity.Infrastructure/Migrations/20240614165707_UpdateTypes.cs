using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c227260d-fd54-4e9b-b646-6fcd16975558", new DateTime(2024, 6, 14, 16, 57, 6, 441, DateTimeKind.Utc).AddTicks(8122), "AQAAAAIAAYagAAAAEI6pG+v9CfV9fJMD6PqcLfpX8FOVGqzbQqwuzKT66dR7wO9orTg/2NAlj8qzvRB+2Q==", "13dde4db-7377-4206-b3ee-fb040c2ea3ef" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9e224968-33e4-4652-b7b7-8574d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "PasswordHash", "SecurityStamp" },
                values: new object[] { "278eacce-7b42-4da4-8380-facd7cb471de", new DateTime(2024, 6, 14, 16, 57, 6, 371, DateTimeKind.Utc).AddTicks(9910), "AQAAAAIAAYagAAAAEHmHwlls4un1BTRKHMDOxJVZfIp6BLomGrMWKk76/UF0ZlXA7JdhGhR+RJYucXfD3w==", "5d8fb982-43fb-4fcc-936f-32adeb37bcc0" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
