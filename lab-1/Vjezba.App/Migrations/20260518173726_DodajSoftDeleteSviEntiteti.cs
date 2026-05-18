using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vjezba.App.Migrations
{
    /// <inheritdoc />
    public partial class DodajSoftDeleteSviEntiteti : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "ZaduzenjaOpreme",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "ServisniZahtjevi",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Radnici",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Proizvodaci",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Odrzavanja",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Lokacije",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "KategorijeOpreme",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ZaduzenjaOpreme");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ServisniZahtjevi");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Radnici");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Proizvodaci");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Odrzavanja");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Lokacije");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "KategorijeOpreme");
        }
    }
}
