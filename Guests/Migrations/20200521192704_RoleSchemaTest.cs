using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Guests.Migrations
{
    public partial class RoleSchemaTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "topics",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2020, 5, 21, 14, 27, 4, 465, DateTimeKind.Local).AddTicks(4533), new DateTime(2020, 5, 21, 14, 27, 4, 468, DateTimeKind.Local).AddTicks(1559) });

            migrationBuilder.UpdateData(
                table: "topics",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2020, 5, 21, 14, 27, 4, 469, DateTimeKind.Local).AddTicks(2245), new DateTime(2020, 5, 21, 14, 27, 4, 469, DateTimeKind.Local).AddTicks(2271) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "topics",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2020, 5, 14, 8, 57, 43, 508, DateTimeKind.Local).AddTicks(5300), new DateTime(2020, 5, 14, 8, 57, 43, 511, DateTimeKind.Local).AddTicks(609) });

            migrationBuilder.UpdateData(
                table: "topics",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2020, 5, 14, 8, 57, 43, 512, DateTimeKind.Local).AddTicks(635), new DateTime(2020, 5, 14, 8, 57, 43, 512, DateTimeKind.Local).AddTicks(662) });
        }
    }
}
