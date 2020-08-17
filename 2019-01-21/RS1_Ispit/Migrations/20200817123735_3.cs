using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RS1_Ispit_asp.net_core.Migrations
{
    public partial class _3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Prosjek",
                table: "MaturskiIspitStavka",
                nullable: false,
                oldClrType: typeof(float));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Prosjek",
                table: "MaturskiIspitStavka",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
