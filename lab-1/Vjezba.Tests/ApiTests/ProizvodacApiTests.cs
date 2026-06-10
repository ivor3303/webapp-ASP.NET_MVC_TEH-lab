using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Vjezba.App.Data;
using Vjezba.App.DTOs;
using Vjezba.Model;

namespace Vjezba.Tests.ApiTests;

public class ProizvodacApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public ProizvodacApiTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetAll_ReturnsOkAndList()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        ApiTestSeeders.ResetDatabase(db);
        db.Proizvodaci.Add(new Proizvodac { Naziv = "Bosch", Drzava = "DE", KontaktEmail = "info@bosch.de" });
        db.SaveChanges();

        var response = await _factory.CreateClient().GetAsync("/api/proizvodac");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.Content.ReadFromJsonAsync<List<ProizvodacDTO>>();
        items.Should().NotBeNull();
        items.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenExists()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        ApiTestSeeders.ResetDatabase(db);
        var entity = new Proizvodac { Naziv = "Makita", Drzava = "JP", KontaktEmail = "info@makita.jp" };
        db.Proizvodaci.Add(entity);
        db.SaveChanges();

        var response = await _factory.CreateClient().GetAsync($"/api/proizvodac/{entity.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var item = await response.Content.ReadFromJsonAsync<ProizvodacDTO>();
        item!.Id.Should().Be(entity.Id);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenNotExists()
    {
        var response = await _factory.CreateClient().GetAsync("/api/proizvodac/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Post_CreatesProizvodac_ReturnsCreated()
    {
        var response = await _factory.CreateClient().PostAsJsonAsync("/api/proizvodac", new Proizvodac
        {
            Naziv = "DeWalt",
            Drzava = "US",
            KontaktEmail = "info@dewalt.com"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_ReturnsBadRequest_WhenInvalidModel()
    {
        var response = await _factory.CreateClient().PostAsJsonAsync("/api/proizvodac", new { });
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Put_UpdatesProizvodac_ReturnsOk()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        ApiTestSeeders.ResetDatabase(db);
        var entity = new Proizvodac { Naziv = "Stari", Drzava = "HR", KontaktEmail = "stari@test.hr" };
        db.Proizvodaci.Add(entity);
        db.SaveChanges();

        entity.Naziv = "Azurirani";

        var response = await _factory.CreateClient().PutAsJsonAsync($"/api/proizvodac/{entity.Id}", entity);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Put_ReturnsNotFound_WhenNotExists()
    {
        var response = await _factory.CreateClient().PutAsJsonAsync("/api/proizvodac/99999", new Proizvodac
        {
            Naziv = "X",
            Drzava = "HR",
            KontaktEmail = "x@x.hr"
        });
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_HardDeletes_ReturnsOk()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        ApiTestSeeders.ResetDatabase(db);
        var entity = new Proizvodac { Naziv = "Za brisanje", Drzava = "HR", KontaktEmail = "brisanje@test.hr" };
        db.Proizvodaci.Add(entity);
        db.SaveChanges();

        var response = await _factory.CreateClient().DeleteAsync($"/api/proizvodac/{entity.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        using var verifyScope = _factory.Services.CreateScope();
        var verifyDb = verifyScope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        var exists = await verifyDb.Proizvodaci.AnyAsync(x => x.Id == entity.Id);
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenNotExists()
    {
        var response = await _factory.CreateClient().DeleteAsync("/api/proizvodac/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
