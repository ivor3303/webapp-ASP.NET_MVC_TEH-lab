using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Vjezba.App.Data;
using Vjezba.App.DTOs;
using Vjezba.Model;

namespace Vjezba.Tests.ApiTests;

public class LokacijaApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public LokacijaApiTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetAll_ReturnsOkAndList()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        ApiTestSeeders.ResetDatabase(db);
        var seeded = ApiTestSeeders.SeedLokacija(db, "1");

        var response = await _factory.CreateClient().GetAsync("/api/lokacija");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.Content.ReadFromJsonAsync<List<LokacijaDTO>>();
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
        var seeded = ApiTestSeeders.SeedLokacija(db, "2");

        var response = await _factory.CreateClient().GetAsync($"/api/lokacija/{seeded.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var item = await response.Content.ReadFromJsonAsync<LokacijaDTO>();
        item.Should().NotBeNull();
        item!.Id.Should().Be(seeded.Id);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenNotExists()
    {
        var response = await _factory.CreateClient().GetAsync("/api/lokacija/99999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Post_CreatesLokacija_ReturnsCreated()
    {
        var response = await _factory.CreateClient().PostAsJsonAsync("/api/lokacija", new Lokacija
        {
            Naziv = "Nova lokacija",
            Adresa = "Ulica 1"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_ReturnsBadRequest_WhenInvalidModel()
    {
        var response = await _factory.CreateClient().PostAsJsonAsync("/api/lokacija", new { });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Put_UpdatesLokacija_ReturnsOk()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        ApiTestSeeders.ResetDatabase(db);
        var seeded = ApiTestSeeders.SeedLokacija(db, "3");
        seeded.Naziv = "Azurirana lokacija";

        var response = await _factory.CreateClient().PutAsJsonAsync($"/api/lokacija/{seeded.Id}", seeded);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Put_ReturnsNotFound_WhenNotExists()
    {
        var response = await _factory.CreateClient().PutAsJsonAsync("/api/lokacija/99999", new Lokacija
        {
            Naziv = "A",
            Adresa = "B"
        });

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_HardDeletes_ReturnsOk()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        ApiTestSeeders.ResetDatabase(db);
        var seeded = ApiTestSeeders.SeedLokacija(db, "4");

        var response = await _factory.CreateClient().DeleteAsync($"/api/lokacija/{seeded.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        using var verificationScope = _factory.Services.CreateScope();
        var verificationDb = verificationScope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        var exists = await verificationDb.Lokacije.AnyAsync(x => x.Id == seeded.Id);
        exists.Should().BeFalse();
    }
}