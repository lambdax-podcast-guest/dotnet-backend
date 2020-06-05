using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Guests.Migrations
{
    public partial class LoopFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_podcast_hosts_users_host_id",
                table: "podcast_hosts");

            migrationBuilder.UpdateData(
                table: "topics",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2020, 6, 5, 9, 51, 25, 13, DateTimeKind.Local).AddTicks(8532), new DateTime(2020, 6, 5, 9, 51, 25, 17, DateTimeKind.Local).AddTicks(4055) });

            migrationBuilder.UpdateData(
                table: "topics",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2020, 6, 5, 9, 51, 25, 18, DateTimeKind.Local).AddTicks(8774), new DateTime(2020, 6, 5, 9, 51, 25, 18, DateTimeKind.Local).AddTicks(8810) });

            migrationBuilder.AddForeignKey(
                name: "fk_podcast_hosts_users_user_id",
                table: "podcast_hosts",
                column: "host_id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_podcast_hosts_users_user_id",
                table: "podcast_hosts");

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

            migrationBuilder.AddForeignKey(
                name: "fk_podcast_hosts_users_host_id",
                table: "podcast_hosts",
                column: "host_id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
