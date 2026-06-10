using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Vjezba.App.Data;
using Vjezba.App.DTOs;
using Vjezba.Model;

namespace Vjezba.Tests.ApiTests;

public class ZaduzenjeOpremeApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public ZaduzenjeOpremeApiTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetAll_ReturnsOkAndList()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        ApiTestSeeders.ResetDatabase(db);
        var radnik = ApiTestSeeders.SeedRadnik(db, "zo1");
        var oprema = ApiTestSeeders.SeedRadnaOprema(db, "zo1");
        db.ZaduzenjaOpreme.Add(new ZaduzenjeOpreme
        {
            RadnikId = radnik.Id,
            RadnaOpremaId = oprema.Id,
            DatumZaduzenja = new DateTime(2024, 1, 1)
        });
        db.SaveChanges();

        var response = await _factory.CreateClient().GetAsync("/api/zaduzenjeOpreme");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.Content.ReadFromJsonAsync<List<ZaduzenjeOpremeDTO>>();
        items.Should().NotBeNull();
        items.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenExists()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        ApiTestSeeders.ResetDatabase(db);
        var radnik = ApiTestSeeders.SeedRadnik(db, "zo2");
        var oprema = ApiTestSeeders.SeedRadnaOprema(db, "zo2");
        var entity = new ZaduzenjeOpreme
        {
            RadnikId = radnik.Id,
            RadnaOpremaId = oprema.Id,
            DatumZaduzenja = new DateTime(2024, 2, 1)
        };
        db.ZaduzenjaOpreme.Add(entity);
        db.SaveChanges();

        var response = await _factory.CreateClient().GetAsync($"/api/zaduzenjeOpreme/{entity.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var item = await response.Content.ReadFromJsonAsync<ZaduzenjeOpremeDTO>();
        item!.Id.Should().Be(entity.Id);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenNotExists()
    {
        var response = await _factory.CreateClient().GetAsync("/api/zaduzenjeOpreme/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Post_CreatesZaduzenje_ReturnsCreated()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        ApiTestSeeders.ResetDatabase(db);
        var radnik = ApiTestSeeders.SeedRadnik(db, "zo3");
        var oprema = ApiTestSeeders.SeedRadnaOprema(db, "zo3");

        var response = await _factory.CreateClient().PostAsJsonAsync("/api/zaduzenjeOpreme", new
        {
            RadnikId = radnik.Id,
            RadnaOpremaId = oprema.Id,
            DatumZaduzenja = new DateTime(2024, 3, 1)
        });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_ReturnsErrorForMissingForeignKeys()
    {
        // ZaduzenjeOpreme nema [Required] string polja — FK violation vraca 500
        var response = await _factory.CreateClient().PostAsJsonAsync("/api/zaduzenjeOpreme", new
        {
            RadnikId = 99999,
            RadnaOpremaId = 99999,
            DatumZaduzenja = new DateTime(2024, 1, 1)
        });

        response.StatusCode.Should().NotBe(HttpStatusCode.OK);
        response.StatusCode.Should().NotBe(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Put_UpdatesZaduzenje_ReturnsOk()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        ApiTestSeeders.ResetDatabase(db);
        var radnik = ApiTestSeeders.SeedRadnik(db, "zo4");
        var oprema = ApiTestSeeders.SeedRadnaOprema(db, "zo4");
        var entity = new ZaduzenjeOpreme
        {
            RadnikId = radnik.Id,
            RadnaOpremaId = oprema.Id,
            DatumZaduzenja = new DateTime(2024, 4, 1)
        };
        db.ZaduzenjaOpreme.Add(entity);
        db.SaveChanges();

        var response = await _factory.CreateClient().PutAsJsonAsync($"/api/zaduzenjeOpreme/{entity.Id}", new
        {
            RadnikId = radnik.Id,
            RadnaOpremaId = oprema.Id,
            DatumZaduzenja = new DateTime(2024, 4, 1),
            DatumRazduzenja = new DateTime(2024, 5, 1)
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Put_ReturnsNotFound_WhenNotExists()
    {
        var response = await _factory.CreateClient().PutAsJsonAsync("/api/zaduzenjeOpreme/99999", new
        {
            RadnikId = 1,
            RadnaOpremaId = 1,
            DatumZaduzenja = new DateTime(2024, 1, 1)
        });
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_HardDeletes_ReturnsOk()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        ApiTestSeeders.ResetDatabase(db);
        var radnik = ApiTestSeeders.SeedRadnik(db, "zo5");
        var oprema = ApiTestSeeders.SeedRadnaOprema(db, "zo5");
        var entity = new ZaduzenjeOpreme
        {
            RadnikId = radnik.Id,
            RadnaOpremaId = oprema.Id,
            DatumZaduzenja = new DateTime(2024, 5, 1)
        };
        db.ZaduzenjaOpreme.Add(entity);
        db.SaveChanges();

        var response = await _factory.CreateClient().DeleteAsync($"/api/zaduzenjeOpreme/{entity.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        using var verifyScope = _factory.Services.CreateScope();
        var verifyDb = verifyScope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        var exists = await verifyDb.ZaduzenjaOpreme.AnyAsync(x => x.Id == entity.Id);
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenNotExists()
    {
        var response = await _factory.CreateClient().DeleteAsync("/api/zaduzenjeOpreme/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}