using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WorldCapBetService.Migrations
{
    public partial class NotNullMatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Match_Team_Team1Id",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Team_Team2Id",
                table: "Match");

            migrationBuilder.AlterColumn<long>(
                name: "Team2Id",
                table: "Match",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Team1Id",
                table: "Match",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Team_Team1Id",
                table: "Match",
                column: "Team1Id",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Team_Team2Id",
                table: "Match",
                column: "Team2Id",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Match_Team_Team1Id",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Team_Team2Id",
                table: "Match");

            migrationBuilder.AlterColumn<long>(
                name: "Team2Id",
                table: "Match",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "Team1Id",
                table: "Match",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Team_Team1Id",
                table: "Match",
                column: "Team1Id",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Team_Team2Id",
                table: "Match",
                column: "Team2Id",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
