using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Vjezba.App.Data;
using Vjezba.App.DTOs;
using Vjezba.Model;

namespace Vjezba.Tests.ApiTests;

public class ServisniZahtjevApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public ServisniZahtjevApiTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetAll_ReturnsOkAndList()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        ApiTestSeeders.ResetDatabase(db);
        var oprema = ApiTestSeeders.SeedRadnaOprema(db, "sz1");
        db.ServisniZahtjevi.Add(new ServisniZahtjev
        {
            DatumPrijave = new DateTime(2024, 1, 1),
            OpisKvara = "Kvar 1",
            Hitno = false,
            Komentar = "Komentar 1",
            OpremaId = oprema.Id
        });
        db.SaveChanges();

        var response = await _factory.CreateClient().GetAsync("/api/servisniZahtjev");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.Content.ReadFromJsonAsync<List<ServisniZahtjevDTO>>();
        items.Should().NotBeNull();
        items.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenExists()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        ApiTestSeeders.ResetDatabase(db);
        var oprema = ApiTestSeeders.SeedRadnaOprema(db, "sz2");
        var entity = new ServisniZahtjev
        {
            DatumPrijave = new DateTime(2024, 2, 1),
            OpisKvara = "Kvar 2",
            Hitno = true,
            Komentar = "Komentar 2",
            OpremaId = oprema.Id
        };
        db.ServisniZahtjevi.Add(entity);
        db.SaveChanges();

        var response = await _factory.CreateClient().GetAsync($"/api/servisniZahtjev/{entity.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var item = await response.Content.ReadFromJsonAsync<ServisniZahtjevDTO>();
        item!.Id.Should().Be(entity.Id);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenNotExists()
    {
        var response = await _factory.CreateClient().GetAsync("/api/servisniZahtjev/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Post_CreatesServisniZahtjev_ReturnsCreated()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        ApiTestSeeders.ResetDatabase(db);
        var oprema = ApiTestSeeders.SeedRadnaOprema(db, "sz3");

        var response = await _factory.CreateClient().PostAsJsonAsync("/api/servisniZahtjev", new
        {
            DatumPrijave = new DateTime(2024, 3, 1),
            OpisKvara = "Novi kvar",
            Hitno = false,
            Komentar = "Novi komentar",
            OpremaId = oprema.Id
        });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_ReturnsBadRequest_WhenInvalidModel()
    {
        var response = await _factory.CreateClient().PostAsJsonAsync("/api/servisniZahtjev", new { });
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Put_UpdatesServisniZahtjev_ReturnsOk()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        ApiTestSeeders.ResetDatabase(db);
        var oprema = ApiTestSeeders.SeedRadnaOprema(db, "sz4");
        var entity = new ServisniZahtjev
        {
            DatumPrijave = new DateTime(2024, 4, 1),
            OpisKvara = "Stari kvar",
            Hitno = false,
            Komentar = "Stari komentar",
            OpremaId = oprema.Id
        };
        db.ServisniZahtjevi.Add(entity);
        db.SaveChanges();

        var response = await _factory.CreateClient().PutAsJsonAsync($"/api/servisniZahtjev/{entity.Id}", new
        {
            DatumPrijave = new DateTime(2024, 4, 1),
            OpisKvara = "Azurirani kvar",
            Hitno = true,
            Komentar = "Azurirani komentar",
            OpremaId = oprema.Id
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Put_ReturnsNotFound_WhenNotExists()
    {
        var response = await _factory.CreateClient().PutAsJsonAsync("/api/servisniZahtjev/99999", new
        {
            DatumPrijave = new DateTime(2024, 1, 1),
            OpisKvara = "X",
            Hitno = false,
            Komentar = "X",
            OpremaId = 1
        });
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_HardDeletes_ReturnsOk()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        ApiTestSeeders.ResetDatabase(db);
        var oprema = ApiTestSeeders.SeedRadnaOprema(db, "sz5");
        var entity = new ServisniZahtjev
        {
            DatumPrijave = new DateTime(2024, 5, 1),
            OpisKvara = "Za brisanje",
            Hitno = false,
            Komentar = "Brisanje",
            OpremaId = oprema.Id
        };
        db.ServisniZahtjevi.Add(entity);
        db.SaveChanges();

        var response = await _factory.CreateClient().DeleteAsync($"/api/servisniZahtjev/{entity.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        using var verifyScope = _factory.Services.CreateScope();
        var verifyDb = verifyScope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        var exists = await verifyDb.ServisniZahtjevi.AnyAsync(x => x.Id == entity.Id);
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenNotExists()
    {
        var response = await _factory.CreateClient().DeleteAsync("/api/servisniZahtjev/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}