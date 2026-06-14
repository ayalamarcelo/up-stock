using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarModeloUsuarioAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "assets",
                columns: table => new
                {
                    assetid = table.Column<Guid>(type: "uuid", nullable: false),
                    categoryid = table.Column<Guid>(type: "uuid", nullable: false),
                    statusid = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    codeid = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_assets", x => x.assetid);
                });

            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    namecategory = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_category", x => x.category_id);
                });

            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    clientid = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    dnicuit = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    isactive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clients", x => x.clientid);
                });

            migrationBuilder.CreateTable(
                name: "status",
                columns: table => new
                {
                    statusid = table.Column<Guid>(type: "uuid", nullable: false),
                    namestatus = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_status", x => x.statusid);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    userid = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    passwordhash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    rol = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    isactive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.userid);
                });

            migrationBuilder.CreateTable(
                name: "rental",
                columns: table => new
                {
                    rentalid = table.Column<Guid>(type: "uuid", nullable: false),
                    statusid = table.Column<Guid>(type: "uuid", nullable: false),
                    clientid = table.Column<Guid>(type: "uuid", nullable: false),
                    userid = table.Column<Guid>(type: "uuid", nullable: false),
                    rentaldate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    rentaldateexpected = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status_id = table.Column<Guid>(type: "uuid", nullable: true),
                    client_id = table.Column<Guid>(type: "uuid", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rental", x => x.rentalid);
                    table.ForeignKey(
                        name: "fk_rental_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "clientid");
                    table.ForeignKey(
                        name: "fk_rental_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "statusid");
                    table.ForeignKey(
                        name: "fk_rental_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "userid");
                });

            migrationBuilder.CreateTable(
                name: "rental_items",
                columns: table => new
                {
                    rentalitemid = table.Column<Guid>(type: "uuid", nullable: false),
                    rentalid = table.Column<Guid>(type: "uuid", nullable: false),
                    assetid = table.Column<Guid>(type: "uuid", nullable: false),
                    rental_id = table.Column<Guid>(type: "uuid", nullable: true),
                    asset_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rental_items", x => x.rentalitemid);
                    table.ForeignKey(
                        name: "fk_rental_items_assets_asset_id",
                        column: x => x.asset_id,
                        principalTable: "assets",
                        principalColumn: "assetid");
                    table.ForeignKey(
                        name: "fk_rental_items_rental_rental_id",
                        column: x => x.rental_id,
                        principalTable: "rental",
                        principalColumn: "rentalid");
                });

            migrationBuilder.CreateIndex(
                name: "ix_rental_client_id",
                table: "rental",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_rental_status_id",
                table: "rental",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "ix_rental_user_id",
                table: "rental",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_rental_items_asset_id",
                table: "rental_items",
                column: "asset_id");

            migrationBuilder.CreateIndex(
                name: "ix_rental_items_rental_id",
                table: "rental_items",
                column: "rental_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "rental_items");

            migrationBuilder.DropTable(
                name: "assets");

            migrationBuilder.DropTable(
                name: "rental");

            migrationBuilder.DropTable(
                name: "clients");

            migrationBuilder.DropTable(
                name: "status");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
