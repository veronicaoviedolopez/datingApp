﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace datingApp.API.Migrations
{
    public partial class MessagesEntityAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    senderId = table.Column<int>(nullable: false),
                    recipientId = table.Column<int>(nullable: false),
                    content = table.Column<string>(nullable: true),
                    isRead = table.Column<bool>(nullable: false),
                    dateRead = table.Column<DateTime>(nullable: true),
                    messageSent = table.Column<DateTime>(nullable: false),
                    senderDeleted = table.Column<bool>(nullable: false),
                    recipientDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.id);
                    table.ForeignKey(
                        name: "FK_Messages_Users_recipientId",
                        column: x => x.recipientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Users_senderId",
                        column: x => x.senderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_recipientId",
                table: "Messages",
                column: "recipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_senderId",
                table: "Messages",
                column: "senderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");
        }
    }
}
