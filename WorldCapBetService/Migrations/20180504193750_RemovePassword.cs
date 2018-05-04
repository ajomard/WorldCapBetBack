using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WorldCapBetService.Migrations
{
    public partial class RemovePassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Match_Team_Team1Id",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Team_Team2Id",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Pronostic_Match_MatchId",
                table: "Pronostic");

            migrationBuilder.DropForeignKey(
                name: "FK_Pronostic_User_UserId",
                table: "Pronostic");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Pronostic",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<long>(
                name: "MatchId",
                table: "Pronostic",
                nullable: true,
                oldClrType: typeof(long));

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

            migrationBuilder.AddForeignKey(
                name: "FK_Pronostic_Match_MatchId",
                table: "Pronostic",
                column: "MatchId",
                principalTable: "Match",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pronostic_User_UserId",
                table: "Pronostic",
                column: "UserId",
                principalTable: "User",
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

            migrationBuilder.DropForeignKey(
                name: "FK_Pronostic_Match_MatchId",
                table: "Pronostic");

            migrationBuilder.DropForeignKey(
                name: "FK_Pronostic_User_UserId",
                table: "Pronostic");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "User",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Pronostic",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "MatchId",
                table: "Pronostic",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

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
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Team_Team2Id",
                table: "Match",
                column: "Team2Id",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pronostic_Match_MatchId",
                table: "Pronostic",
                column: "MatchId",
                principalTable: "Match",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pronostic_User_UserId",
                table: "Pronostic",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
