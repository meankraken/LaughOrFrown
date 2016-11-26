using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LaughOrFrown.Migrations
{
    public partial class AddRatings4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Jokes_JokeRatedId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_JokeRatedId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "JokeRatedId",
                table: "Ratings");

            migrationBuilder.AddColumn<int>(
                name: "JokeId",
                table: "Ratings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Ratings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_JokeId",
                table: "Ratings",
                column: "JokeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Jokes_JokeId",
                table: "Ratings",
                column: "JokeId",
                principalTable: "Jokes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Jokes_JokeId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_JokeId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "JokeId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Ratings");

            migrationBuilder.AddColumn<int>(
                name: "JokeRatedId",
                table: "Ratings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_JokeRatedId",
                table: "Ratings",
                column: "JokeRatedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Jokes_JokeRatedId",
                table: "Ratings",
                column: "JokeRatedId",
                principalTable: "Jokes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
