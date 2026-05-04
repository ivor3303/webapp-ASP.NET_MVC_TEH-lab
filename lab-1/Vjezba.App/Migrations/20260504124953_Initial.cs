using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vjezba.App.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KategorijeOpreme",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naziv = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Opis = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KategorijeOpreme", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lokacije",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naziv = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Adresa = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lokacije", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proizvodaci",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naziv = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Drzava = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    KontaktEmail = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proizvodaci", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Radnici",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ime = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Prezime = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    RadnoMjesto = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Telefon = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DatumZaposlenja = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Aktivan = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Radnici", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RadnaOprema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naziv = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    InventarniBroj = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    SerijskiBroj = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    DatumNabave = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    LokacijaId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProizvodacId = table.Column<int>(type: "INTEGER", nullable: false),
                    KategorijaOpremeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RadnaOprema", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RadnaOprema_KategorijeOpreme_KategorijaOpremeId",
                        column: x => x.KategorijaOpremeId,
                        principalTable: "KategorijeOpreme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RadnaOprema_Lokacije_LokacijaId",
                        column: x => x.LokacijaId,
                        principalTable: "Lokacije",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RadnaOprema_Proizvodaci_ProizvodacId",
                        column: x => x.ProizvodacId,
                        principalTable: "Proizvodaci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Odrzavanja",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Datum = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Opis = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Cijena = table.Column<decimal>(type: "TEXT", nullable: false),
                    IzvrsioId = table.Column<int>(type: "INTEGER", nullable: false),
                    TrajanjeTicks = table.Column<long>(type: "INTEGER", nullable: false),
                    OpremaId = table.Column<int>(type: "INTEGER", nullable: false),
                    Napomena = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Odrzavanja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Odrzavanja_RadnaOprema_OpremaId",
                        column: x => x.OpremaId,
                        principalTable: "RadnaOprema",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Odrzavanja_Radnici_IzvrsioId",
                        column: x => x.IzvrsioId,
                        principalTable: "Radnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServisniZahtjevi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DatumPrijave = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OpisKvara = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Hitno = table.Column<bool>(type: "INTEGER", nullable: false),
                    OpremaId = table.Column<int>(type: "INTEGER", nullable: false),
                    Komentar = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    RadnikId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServisniZahtjevi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServisniZahtjevi_RadnaOprema_OpremaId",
                        column: x => x.OpremaId,
                        principalTable: "RadnaOprema",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServisniZahtjevi_Radnici_RadnikId",
                        column: x => x.RadnikId,
                        principalTable: "Radnici",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ZaduzenjaOpreme",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RadnikId = table.Column<int>(type: "INTEGER", nullable: false),
                    RadnaOpremaId = table.Column<int>(type: "INTEGER", nullable: false),
                    DatumZaduzenja = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DatumRazduzenja = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZaduzenjaOpreme", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZaduzenjaOpreme_RadnaOprema_RadnaOpremaId",
                        column: x => x.RadnaOpremaId,
                        principalTable: "RadnaOprema",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ZaduzenjaOpreme_Radnici_RadnikId",
                        column: x => x.RadnikId,
                        principalTable: "Radnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Odrzavanja_IzvrsioId",
                table: "Odrzavanja",
                column: "IzvrsioId");

            migrationBuilder.CreateIndex(
                name: "IX_Odrzavanja_OpremaId",
                table: "Odrzavanja",
                column: "OpremaId");

            migrationBuilder.CreateIndex(
                name: "IX_RadnaOprema_KategorijaOpremeId",
                table: "RadnaOprema",
                column: "KategorijaOpremeId");

            migrationBuilder.CreateIndex(
                name: "IX_RadnaOprema_LokacijaId",
                table: "RadnaOprema",
                column: "LokacijaId");

            migrationBuilder.CreateIndex(
                name: "IX_RadnaOprema_ProizvodacId",
                table: "RadnaOprema",
                column: "ProizvodacId");

            migrationBuilder.CreateIndex(
                name: "IX_ServisniZahtjevi_OpremaId",
                table: "ServisniZahtjevi",
                column: "OpremaId");

            migrationBuilder.CreateIndex(
                name: "IX_ServisniZahtjevi_RadnikId",
                table: "ServisniZahtjevi",
                column: "RadnikId");

            migrationBuilder.CreateIndex(
                name: "IX_ZaduzenjaOpreme_RadnaOpremaId",
                table: "ZaduzenjaOpreme",
                column: "RadnaOpremaId");

            migrationBuilder.CreateIndex(
                name: "IX_ZaduzenjaOpreme_RadnikId",
                table: "ZaduzenjaOpreme",
                column: "RadnikId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Odrzavanja");

            migrationBuilder.DropTable(
                name: "ServisniZahtjevi");

            migrationBuilder.DropTable(
                name: "ZaduzenjaOpreme");

            migrationBuilder.DropTable(
                name: "RadnaOprema");

            migrationBuilder.DropTable(
                name: "Radnici");

            migrationBuilder.DropTable(
                name: "KategorijeOpreme");

            migrationBuilder.DropTable(
                name: "Lokacije");

            migrationBuilder.DropTable(
                name: "Proizvodaci");
        }
    }
}
