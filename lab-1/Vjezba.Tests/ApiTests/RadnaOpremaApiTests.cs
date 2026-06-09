using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Vjezba.App.Data;
using Vjezba.App.DTOs;
using Vjezba.Model;

namespace Vjezba.Tests.ApiTests;

public class RadnaOpremaApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public RadnaOpremaApiTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private VjezbaDbContext GetDb()
    {
        var scope = _factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
    }

    [Fact]
    public async Task GetAll_ReturnsOkAndList()
    {
        var db = GetDb();
        var seeded = ApiTestSeeders.SeedRadnaOprema(db, "getall");

        var response = await _client.GetAsync("/api/radnaOprema");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.Content.ReadFromJsonAsync<List<RadnaOpremaDTO>>();
        items.Should().NotBeNull();
        items.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenExists()
    {
        var db = GetDb();
        var seeded = ApiTestSeeders.SeedRadnaOprema(db, "getbyid");

        var response = await _client.GetAsync($"/api/radnaOprema/{seeded.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var item = await response.Content.ReadFromJsonAsync<RadnaOpremaDTO>();
        item.Should().NotBeNull();
        item!.Id.Should().Be(seeded.Id);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenNotExists()
    {
        var response = await _client.GetAsync("/api/radnaOprema/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Post_CreatesOprema_ReturnsCreated()
    {
        var db = GetDb();
        var lokacija = ApiTestSeeders.SeedLokacija(db, "post1");
        var proizvodac = ApiTestSeeders.SeedProizvodac(db, "post1");
        var kategorija = ApiTestSeeders.SeedKategorijaOpreme(db, "post1");

        var payload = new
        {
            Naziv = "Nova oprema",
            InventarniBroj = "INV-POST",
            SerijskiBroj = "SN-POST",
            DatumNabave = new DateTime(2024, 2, 20),
            Status = 0,
            LokacijaId = lokacija.Id,
            ProizvodacId = proizvodac.Id,
            KategorijaOpremeId = kategorija.Id
        };

        var response = await _client.PostAsJsonAsync("/api/radnaOprema", payload);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_ReturnsBadRequest_WhenInvalidModel()
    {
        var response = await _client.PostAsJsonAsync("/api/radnaOprema", new { });
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Put_UpdatesOprema_ReturnsOk()
    {
        var db = GetDb();
        var seeded = ApiTestSeeders.SeedRadnaOprema(db, "put1");

        var payload = new
        {
            Id = seeded.Id,
            Naziv = "Azurirana oprema",
            InventarniBroj = seeded.InventarniBroj,
            SerijskiBroj = seeded.SerijskiBroj,
            DatumNabave = seeded.DatumNabave,
            Status = (int)seeded.Status,
            LokacijaId = seeded.LokacijaId,
            ProizvodacId = seeded.ProizvodacId,
            KategorijaOpremeId = seeded.KategorijaOpremeId
        };

        var response = await _client.PutAsJsonAsync($"/api/radnaOprema/{seeded.Id}", payload);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Put_ReturnsNotFound_WhenNotExists()
    {
        var payload = new
        {
            Naziv = "Azurirano",
            InventarniBroj = "INV-1",
            SerijskiBroj = "SN-1",
            DatumNabave = new DateTime(2024, 1, 1),
            Status = 0,
            LokacijaId = 1,
            ProizvodacId = 1,
            KategorijaOpremeId = 1
        };

        var response = await _client.PutAsJsonAsync("/api/radnaOprema/99999", payload);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_SoftDeletes_ReturnsOk()
    {
        var db = GetDb();
        var seeded = ApiTestSeeders.SeedRadnaOprema(db, "delete1");

        var response = await _client.DeleteAsync($"/api/radnaOprema/{seeded.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var verifyDb = GetDb();
        var entity = await verifyDb.RadnaOprema.FirstAsync(x => x.Id == seeded.Id);
        entity.Status.Should().Be(StatusOpreme.Otpisana);
    }
}