using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Vjezba.App.Data;
using Vjezba.App.DTOs;
using Vjezba.Model;

namespace Vjezba.Tests.ApiTests;

public class RadnikApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public RadnikApiTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetAll_ReturnsOkAndList()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        ApiTestSeeders.ResetDatabase(db);
        var seeded = ApiTestSeeders.SeedRadnik(db, "1");

        var response = await _factory.CreateClient().GetAsync("/api/radnik");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.Content.ReadFromJsonAsync<List<RadnikDTO>>();
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
        var seeded = ApiTestSeeders.SeedRadnik(db, "2");

        var response = await _factory.CreateClient().GetAsync($"/api/radnik/{seeded.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var item = await response.Content.ReadFromJsonAsync<RadnikDTO>();
        item.Should().NotBeNull();
        item!.Id.Should().Be(seeded.Id);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenNotExists()
    {
        var response = await _factory.CreateClient().GetAsync("/api/radnik/99999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Post_CreatesRadnik_ReturnsCreated()
    {
        var client = _factory.CreateClient();
        var payload = new Radnik
        {
            Ime = "Ivan",
            Prezime = "Ivic",
            RadnoMjesto = "Serviser",
            Email = "ivan@example.com",
            Telefon = "0911234567",
            DatumZaposlenja = new DateTime(2024, 3, 1),
            Aktivan = true
        };

        var response = await client.PostAsJsonAsync("/api/radnik", payload);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_ReturnsBadRequest_WhenInvalidModel()
    {
        var response = await _factory.CreateClient().PostAsJsonAsync("/api/radnik", new { });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Put_UpdatesRadnik_ReturnsOk()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        ApiTestSeeders.ResetDatabase(db);
        var seeded = ApiTestSeeders.SeedRadnik(db, "3");

        seeded.Ime = "Mato";
        seeded.Aktivan = false;

        var response = await _factory.CreateClient().PutAsJsonAsync($"/api/radnik/{seeded.Id}", seeded);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Put_ReturnsNotFound_WhenNotExists()
    {
        var payload = new Radnik
        {
            Ime = "Mato",
            Prezime = "Matic",
            RadnoMjesto = "Tehnicar",
            Email = "mato@example.com",
            Telefon = "0911234567",
            DatumZaposlenja = new DateTime(2024, 1, 1),
            Aktivan = true
        };

        var response = await _factory.CreateClient().PutAsJsonAsync("/api/radnik/99999", payload);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_SoftDeletes_ReturnsOk()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        ApiTestSeeders.ResetDatabase(db);
        var seeded = ApiTestSeeders.SeedRadnik(db, "4");

        var response = await _factory.CreateClient().DeleteAsync($"/api/radnik/{seeded.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        using var verificationScope = _factory.Services.CreateScope();
        var verificationDb = verificationScope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        var entity = await verificationDb.Radnici.FirstAsync(x => x.Id == seeded.Id);
        entity.Aktivan.Should().BeFalse();
    }
}