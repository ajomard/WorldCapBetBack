using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WorldCapBetService.Migrations
{
    public partial class ScoreMatchPronosticNotNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ScoreTeam2",
                table: "Pronostic",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ScoreTeam1",
                table: "Pronostic",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ScoreTeam2",
                table: "Pronostic",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ScoreTeam1",
                table: "Pronostic",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
