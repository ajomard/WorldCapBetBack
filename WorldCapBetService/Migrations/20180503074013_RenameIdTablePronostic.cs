using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WorldCapBetService.Migrations
{
    public partial class RenameIdTablePronostic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Pronostic",
                newName: "PronosticId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PronosticId",
                table: "Pronostic",
                newName: "Id");
        }
    }
}
