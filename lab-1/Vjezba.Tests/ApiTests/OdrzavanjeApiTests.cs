using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Vjezba.App.Data;
using Vjezba.App.DTOs;
using Vjezba.Model;

namespace Vjezba.Tests.ApiTests;

public class OdrzavanjeApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public OdrzavanjeApiTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetAll_ReturnsOkAndList()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        ApiTestSeeders.ResetDatabase(db);
        var seeded = ApiTestSeeders.SeedOdrzavanje(db, "1");

        var response = await _factory.CreateClient().GetAsync("/api/odrzavanje");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.Content.ReadFromJsonAsync<List<OdrzavanjeDTO>>();
        items.Should().NotBeNull();
        items.Should().NotBeEmpty();
        items!.Single().Id.Should().Be(seeded.Id);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenExists()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        ApiTestSeeders.ResetDatabase(db);
        var seeded = ApiTestSeeders.SeedOdrzavanje(db, "2");

        var response = await _factory.CreateClient().GetAsync($"/api/odrzavanje/{seeded.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var item = await response.Content.ReadFromJsonAsync<OdrzavanjeDTO>();
        item.Should().NotBeNull();
        item!.Id.Should().Be(seeded.Id);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenNotExists()
    {
        var response = await _factory.CreateClient().GetAsync("/api/odrzavanje/99999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Post_CreatesOdrzavanje_ReturnsCreated()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        ApiTestSeeders.ResetDatabase(db);
        var radnik = ApiTestSeeders.SeedRadnik(db, "3");
        var oprema = ApiTestSeeders.SeedRadnaOprema(db, "3");

        var payload = new Odrzavanje
        {
            Datum = new DateTime(2024, 7, 1),
            Opis = "Novi servis",
            Cijena = 120m,
            Napomena = "Sve u redu",
            OpremaId = oprema.Id,
            IzvrsioId = radnik.Id,
            TrajanjeTicks = TimeSpan.FromHours(2).Ticks
        };

        var response = await _factory.CreateClient().PostAsJsonAsync("/api/odrzavanje", payload);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_ReturnsBadRequest_WhenInvalidModel()
    {
        var response = await _factory.CreateClient().PostAsJsonAsync("/api/odrzavanje", new { });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Put_UpdatesOdrzavanje_ReturnsOk()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        ApiTestSeeders.ResetDatabase(db);
        var seeded = ApiTestSeeders.SeedOdrzavanje(db, "4");

        var payload = new
        {
            Id = seeded.Id,
            Datum = seeded.Datum,
            Opis = "Azurirani opis",
            Cijena = 150m,
            Napomena = seeded.Napomena,
            OpremaId = seeded.OpremaId,
            IzvrsioId = seeded.IzvrsioId,
            TrajanjeTicks = seeded.TrajanjeTicks
        };

        var response = await _factory.CreateClient().PutAsJsonAsync($"/api/odrzavanje/{seeded.Id}", payload);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Put_ReturnsNotFound_WhenNotExists()
    {
        var response = await _factory.CreateClient().PutAsJsonAsync("/api/odrzavanje/99999", new Odrzavanje
        {
            Datum = new DateTime(2024, 1, 1),
            Opis = "Test",
            Cijena = 1m,
            Napomena = "Test",
            OpremaId = 1,
            IzvrsioId = 1,
            TrajanjeTicks = TimeSpan.FromHours(1).Ticks
        });

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_HardDeletes_ReturnsOk()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        ApiTestSeeders.ResetDatabase(db);
        var seeded = ApiTestSeeders.SeedOdrzavanje(db, "5");

        var response = await _factory.CreateClient().DeleteAsync($"/api/odrzavanje/{seeded.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        using var verificationScope = _factory.Services.CreateScope();
        var verificationDb = verificationScope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        var exists = await verificationDb.Odrzavanja.AnyAsync(x => x.Id == seeded.Id);
        exists.Should().BeFalse();
    }
}