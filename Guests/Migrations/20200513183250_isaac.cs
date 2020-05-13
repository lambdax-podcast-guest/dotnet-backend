using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Guests.Migrations
{
    public partial class isaac : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_Roles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AppUser_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AppUser_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_Roles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AppUser_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AppUser_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Guests",
                table: "Guests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUser",
                table: "AppUser");

            migrationBuilder.RenameTable(
                name: "Guests",
                newName: "guests");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Roles",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Roles",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "NormalizedName",
                table: "Roles",
                newName: "normalized_name");

            migrationBuilder.RenameColumn(
                name: "ConcurrencyStamp",
                table: "Roles",
                newName: "concurrency_stamp");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "guests",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "guests",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "guests",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "AspNetUserTokens",
                newName: "value");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AspNetUserTokens",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                newName: "login_provider");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AspNetUserTokens",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "AspNetUserRoles",
                newName: "role_id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AspNetUserRoles",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                newName: "ix_user_roles_role_id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AspNetUserLogins",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "ProviderDisplayName",
                table: "AspNetUserLogins",
                newName: "provider_display_name");

            migrationBuilder.RenameColumn(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                newName: "provider_key");

            migrationBuilder.RenameColumn(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                newName: "login_provider");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                newName: "ix_user_logins_user_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AspNetUserClaims",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AspNetUserClaims",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "ClaimValue",
                table: "AspNetUserClaims",
                newName: "claim_value");

            migrationBuilder.RenameColumn(
                name: "ClaimType",
                table: "AspNetUserClaims",
                newName: "claim_type");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                newName: "ix_user_claims_user_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AspNetRoleClaims",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "AspNetRoleClaims",
                newName: "role_id");

            migrationBuilder.RenameColumn(
                name: "ClaimValue",
                table: "AspNetRoleClaims",
                newName: "claim_value");

            migrationBuilder.RenameColumn(
                name: "ClaimType",
                table: "AspNetRoleClaims",
                newName: "claim_type");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                newName: "ix_role_claims_role_id");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "AppUser",
                newName: "location");

            migrationBuilder.RenameColumn(
                name: "Languages",
                table: "AppUser",
                newName: "languages");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "AppUser",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Bio",
                table: "AppUser",
                newName: "bio");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AppUser",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "AppUser",
                newName: "user_name");

            migrationBuilder.RenameColumn(
                name: "SecurityStamp",
                table: "AppUser",
                newName: "security_stamp");

            migrationBuilder.RenameColumn(
                name: "PhoneNumberConfirmed",
                table: "AppUser",
                newName: "phone_number_confirmed");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "AppUser",
                newName: "phone_number");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "AppUser",
                newName: "password_hash");

            migrationBuilder.RenameColumn(
                name: "NormalizedUserName",
                table: "AppUser",
                newName: "normalized_user_name");

            migrationBuilder.RenameColumn(
                name: "NormalizedEmail",
                table: "AppUser",
                newName: "normalized_email");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "AppUser",
                newName: "last_name");

            migrationBuilder.RenameColumn(
                name: "HeadLine",
                table: "AppUser",
                newName: "head_line");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "AppUser",
                newName: "first_name");

            migrationBuilder.RenameColumn(
                name: "EmailConfirmed",
                table: "AppUser",
                newName: "email_confirmed");

            migrationBuilder.RenameColumn(
                name: "ConcurrencyStamp",
                table: "AppUser",
                newName: "concurrency_stamp");

            migrationBuilder.RenameColumn(
                name: "AvatarUrl",
                table: "AppUser",
                newName: "avatar_url");

            migrationBuilder.AddPrimaryKey(
                name: "pk_roles",
                table: "Roles",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_guests",
                table: "guests",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_tokens",
                table: "AspNetUserTokens",
                columns: new[] { "user_id", "login_provider", "name" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_roles",
                table: "AspNetUserRoles",
                columns: new[] { "user_id", "role_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_logins",
                table: "AspNetUserLogins",
                columns: new[] { "login_provider", "provider_key" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_claims",
                table: "AspNetUserClaims",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_role_claims",
                table: "AspNetRoleClaims",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_users",
                table: "AppUser",
                column: "id");

            migrationBuilder.CreateTable(
                name: "podcasts",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false),
                    name = table.Column<string>(maxLength: 200, nullable: false),
                    description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_podcasts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "topics",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false),
                    name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_topics", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "podcast_hosts",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false),
                    podcast_id = table.Column<int>(nullable: false),
                    host_id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_podcast_hosts", x => x.id);
                    table.ForeignKey(
                        name: "fk_podcast_hosts_users_host_id",
                        column: x => x.host_id,
                        principalTable: "AppUser",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_podcast_hosts_podcasts_podcast_id",
                        column: x => x.podcast_id,
                        principalTable: "podcasts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "guest_topics",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false),
                    guest_id = table.Column<string>(nullable: false),
                    topic_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_guest_topics", x => x.id);
                    table.ForeignKey(
                        name: "fk_guest_topics_users_guest_id",
                        column: x => x.guest_id,
                        principalTable: "AppUser",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_guest_topics_topics_topic_id",
                        column: x => x.topic_id,
                        principalTable: "topics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "podcast_topics",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false),
                    podcast_id = table.Column<int>(nullable: false),
                    topic_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_podcast_topics", x => x.id);
                    table.ForeignKey(
                        name: "fk_podcast_topics_podcasts_podcast_id",
                        column: x => x.podcast_id,
                        principalTable: "podcasts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_podcast_topics_topics_topic_id",
                        column: x => x.topic_id,
                        principalTable: "topics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_guest_topics_guest_id",
                table: "guest_topics",
                column: "guest_id");

            migrationBuilder.CreateIndex(
                name: "ix_guest_topics_topic_id",
                table: "guest_topics",
                column: "topic_id");

            migrationBuilder.CreateIndex(
                name: "ix_podcast_hosts_host_id",
                table: "podcast_hosts",
                column: "host_id");

            migrationBuilder.CreateIndex(
                name: "ix_podcast_hosts_podcast_id",
                table: "podcast_hosts",
                column: "podcast_id");

            migrationBuilder.CreateIndex(
                name: "ix_podcast_topics_podcast_id",
                table: "podcast_topics",
                column: "podcast_id");

            migrationBuilder.CreateIndex(
                name: "ix_podcast_topics_topic_id",
                table: "podcast_topics",
                column: "topic_id");

            migrationBuilder.AddForeignKey(
                name: "fk_role_claims_asp_net_roles_identity_role_id",
                table: "AspNetRoleClaims",
                column: "role_id",
                principalTable: "Roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_claims_asp_net_users_app_user_id",
                table: "AspNetUserClaims",
                column: "user_id",
                principalTable: "AppUser",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_logins_asp_net_users_app_user_id",
                table: "AspNetUserLogins",
                column: "user_id",
                principalTable: "AppUser",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_asp_net_roles_identity_role_id",
                table: "AspNetUserRoles",
                column: "role_id",
                principalTable: "Roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_asp_net_users_app_user_id",
                table: "AspNetUserRoles",
                column: "user_id",
                principalTable: "AppUser",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_tokens_asp_net_users_app_user_id",
                table: "AspNetUserTokens",
                column: "user_id",
                principalTable: "AppUser",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_role_claims_asp_net_roles_identity_role_id",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "fk_user_claims_asp_net_users_app_user_id",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "fk_user_logins_asp_net_users_app_user_id",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_asp_net_roles_identity_role_id",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_asp_net_users_app_user_id",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "fk_user_tokens_asp_net_users_app_user_id",
                table: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "guest_topics");

            migrationBuilder.DropTable(
                name: "podcast_hosts");

            migrationBuilder.DropTable(
                name: "podcast_topics");

            migrationBuilder.DropTable(
                name: "podcasts");

            migrationBuilder.DropTable(
                name: "topics");

            migrationBuilder.DropPrimaryKey(
                name: "pk_roles",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_guests",
                table: "guests");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_tokens",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_roles",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_logins",
                table: "AspNetUserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_claims",
                table: "AspNetUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "pk_role_claims",
                table: "AspNetRoleClaims");

            migrationBuilder.DropPrimaryKey(
                name: "pk_users",
                table: "AppUser");

            migrationBuilder.RenameTable(
                name: "guests",
                newName: "Guests");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Roles",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Roles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "normalized_name",
                table: "Roles",
                newName: "NormalizedName");

            migrationBuilder.RenameColumn(
                name: "concurrency_stamp",
                table: "Roles",
                newName: "ConcurrencyStamp");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Guests",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Guests",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Guests",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "AspNetUserTokens",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "AspNetUserTokens",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "login_provider",
                table: "AspNetUserTokens",
                newName: "LoginProvider");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "AspNetUserTokens",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "role_id",
                table: "AspNetUserRoles",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "AspNetUserRoles",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "ix_user_roles_role_id",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_RoleId");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "AspNetUserLogins",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "provider_display_name",
                table: "AspNetUserLogins",
                newName: "ProviderDisplayName");

            migrationBuilder.RenameColumn(
                name: "provider_key",
                table: "AspNetUserLogins",
                newName: "ProviderKey");

            migrationBuilder.RenameColumn(
                name: "login_provider",
                table: "AspNetUserLogins",
                newName: "LoginProvider");

            migrationBuilder.RenameIndex(
                name: "ix_user_logins_user_id",
                table: "AspNetUserLogins",
                newName: "IX_AspNetUserLogins_UserId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AspNetUserClaims",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "AspNetUserClaims",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "claim_value",
                table: "AspNetUserClaims",
                newName: "ClaimValue");

            migrationBuilder.RenameColumn(
                name: "claim_type",
                table: "AspNetUserClaims",
                newName: "ClaimType");

            migrationBuilder.RenameIndex(
                name: "ix_user_claims_user_id",
                table: "AspNetUserClaims",
                newName: "IX_AspNetUserClaims_UserId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AspNetRoleClaims",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "role_id",
                table: "AspNetRoleClaims",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "claim_value",
                table: "AspNetRoleClaims",
                newName: "ClaimValue");

            migrationBuilder.RenameColumn(
                name: "claim_type",
                table: "AspNetRoleClaims",
                newName: "ClaimType");

            migrationBuilder.RenameIndex(
                name: "ix_role_claims_role_id",
                table: "AspNetRoleClaims",
                newName: "IX_AspNetRoleClaims_RoleId");

            migrationBuilder.RenameColumn(
                name: "location",
                table: "AppUser",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "languages",
                table: "AppUser",
                newName: "Languages");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "AppUser",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "bio",
                table: "AppUser",
                newName: "Bio");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AppUser",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_name",
                table: "AppUser",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "security_stamp",
                table: "AppUser",
                newName: "SecurityStamp");

            migrationBuilder.RenameColumn(
                name: "phone_number_confirmed",
                table: "AppUser",
                newName: "PhoneNumberConfirmed");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                table: "AppUser",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "password_hash",
                table: "AppUser",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "normalized_user_name",
                table: "AppUser",
                newName: "NormalizedUserName");

            migrationBuilder.RenameColumn(
                name: "normalized_email",
                table: "AppUser",
                newName: "NormalizedEmail");

            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "AppUser",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "head_line",
                table: "AppUser",
                newName: "HeadLine");

            migrationBuilder.RenameColumn(
                name: "first_name",
                table: "AppUser",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "email_confirmed",
                table: "AppUser",
                newName: "EmailConfirmed");

            migrationBuilder.RenameColumn(
                name: "concurrency_stamp",
                table: "AppUser",
                newName: "ConcurrencyStamp");

            migrationBuilder.RenameColumn(
                name: "avatar_url",
                table: "AppUser",
                newName: "AvatarUrl");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Guests",
                table: "Guests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUser",
                table: "AppUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_Roles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AppUser_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AppUser_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_Roles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AppUser_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AppUser_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
