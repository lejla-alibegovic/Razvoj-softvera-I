using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RS1_Ispit_asp.net_core.Migrations
{
    public partial class _4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaturskiIspit_SkolskaGodina_SkolskaGodinaId",
                table: "MaturskiIspit");

            migrationBuilder.DropIndex(
                name: "IX_MaturskiIspit_SkolskaGodinaId",
                table: "MaturskiIspit");

            migrationBuilder.DropColumn(
                name: "SkolskaGodinaId",
                table: "MaturskiIspit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SkolskaGodinaId",
                table: "MaturskiIspit",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MaturskiIspit_SkolskaGodinaId",
                table: "MaturskiIspit",
                column: "SkolskaGodinaId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaturskiIspit_SkolskaGodina_SkolskaGodinaId",
                table: "MaturskiIspit",
                column: "SkolskaGodinaId",
                principalTable: "SkolskaGodina",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
