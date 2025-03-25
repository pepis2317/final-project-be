using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    user_name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    user_password = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    user_balance = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    user_profile = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    user_phone_number = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    user_email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    user_address = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    birth_date = table.Column<DateOnly>(type: "date", nullable: true),
                    gender = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    refresh_token = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    refresh_token_expiry_time = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__users__B9BE370F0EA657DD", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SellerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chats_ChatUsers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "ChatUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Chats_ChatUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ChatUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Shop",
                columns: table => new
                {
                    Shop_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Shop_name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Owner_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Rating = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Address = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Shop__5C0F82B8317FE30A", x => x.Shop_id);
                    table.ForeignKey(
                        name: "FK_shop_owner_id",
                        column: x => x.Owner_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_ChatUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "ChatUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item",
                columns: table => new
                {
                    item_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    item_name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    item_desc = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    shop_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    harga_per_item = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    thumbnail = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__item__52020FDDA5F1086C", x => x.item_id);
                    table.ForeignKey(
                        name: "FK_item_shop_id",
                        column: x => x.shop_id,
                        principalTable: "Shop",
                        principalColumn: "Shop_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    Order_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Order_details = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Order_date = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    Buyer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Item_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    total_harga = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__orders__F1FF845341271034", x => x.Order_id);
                    table.ForeignKey(
                        name: "FK__orders__Buyer_id__0D7A0286",
                        column: x => x.Buyer_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__orders__Item_id__0E6E26BF",
                        column: x => x.Item_id,
                        principalTable: "item",
                        principalColumn: "item_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "productImages",
                columns: table => new
                {
                    Image_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Item_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Image = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Is_primary = table.Column<string>(type: "char(50)", unicode: false, fixedLength: true, maxLength: 50, nullable: false, defaultValue: "false")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__productI__3CAC59117B8AA3BA", x => x.Image_id);
                    table.ForeignKey(
                        name: "FK__productIm__Item___1BC821DD",
                        column: x => x.Item_id,
                        principalTable: "item",
                        principalColumn: "item_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chats_SellerId",
                table: "Chats",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_UserId",
                table: "Chats",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_item_shop_id",
                table: "item",
                column: "shop_id");

            migrationBuilder.CreateIndex(
                name: "UQ__item__ACA52A97D059E7C4",
                table: "item",
                column: "item_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatId",
                table: "Messages",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_Buyer_id",
                table: "orders",
                column: "Buyer_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_Item_id",
                table: "orders",
                column: "Item_id");

            migrationBuilder.CreateIndex(
                name: "IX_productImages_Item_id",
                table: "productImages",
                column: "Item_id");

            migrationBuilder.CreateIndex(
                name: "IX_Shop_Owner_id",
                table: "Shop",
                column: "Owner_id");

            migrationBuilder.CreateIndex(
                name: "UQ__Shop__F265FA7AA61DB603",
                table: "Shop",
                column: "Shop_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "unique_user_name",
                table: "users",
                column: "user_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__users__B0FBA212E391237E",
                table: "users",
                column: "user_email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__users__D5D775E81DFBDDF5",
                table: "users",
                column: "user_phone_number",
                unique: true,
                filter: "[user_phone_number] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "productImages");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "item");

            migrationBuilder.DropTable(
                name: "ChatUsers");

            migrationBuilder.DropTable(
                name: "Shop");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
