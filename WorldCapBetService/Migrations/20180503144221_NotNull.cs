using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WorldCapBetService.Migrations
{
    public partial class NotNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pronostic_Match_MatchId",
                table: "Pronostic");

            migrationBuilder.DropForeignKey(
                name: "FK_Pronostic_User_UserId",
                table: "Pronostic");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pronostic_Match_MatchId",
                table: "Pronostic");

            migrationBuilder.DropForeignKey(
                name: "FK_Pronostic_User_UserId",
                table: "Pronostic");

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
    }
}
