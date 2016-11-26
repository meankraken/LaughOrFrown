using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LaughOrFrown.Migrations
{
    public partial class AddRatings5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_UserRatedId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_UserRatedId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "UserRatedId",
                table: "Ratings");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Ratings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserId1",
                table: "Ratings",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_UserId1",
                table: "Ratings",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_UserId1",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_UserId1",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Ratings");

            migrationBuilder.AddColumn<string>(
                name: "UserRatedId",
                table: "Ratings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserRatedId",
                table: "Ratings",
                column: "UserRatedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_UserRatedId",
                table: "Ratings",
                column: "UserRatedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
