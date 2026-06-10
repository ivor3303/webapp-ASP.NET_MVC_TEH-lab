using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Vjezba.App.Data;
using Vjezba.App.DTOs;
using Vjezba.Model;

namespace Vjezba.Tests.ApiTests;

public class KategorijaOpremeApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public KategorijaOpremeApiTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetAll_ReturnsOkAndList()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        ApiTestSeeders.ResetDatabase(db);
        db.KategorijeOpreme.Add(new KategorijaOpreme { Naziv = "Kat 1", Opis = "Opis 1" });
        db.SaveChanges();

        var response = await _factory.CreateClient().GetAsync("/api/kategorijaOpreme");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.Content.ReadFromJsonAsync<List<KategorijaOpremeDTO>>();
        items.Should().NotBeNull();
        items.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenExists()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        ApiTestSeeders.ResetDatabase(db);
        var entity = new KategorijaOpreme { Naziv = "Kat 2", Opis = "Opis 2" };
        db.KategorijeOpreme.Add(entity);
        db.SaveChanges();

        var response = await _factory.CreateClient().GetAsync($"/api/kategorijaOpreme/{entity.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var item = await response.Content.ReadFromJsonAsync<KategorijaOpremeDTO>();
        item!.Id.Should().Be(entity.Id);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenNotExists()
    {
        var response = await _factory.CreateClient().GetAsync("/api/kategorijaOpreme/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Post_CreatesKategorija_ReturnsCreated()
    {
        var response = await _factory.CreateClient().PostAsJsonAsync("/api/kategorijaOpreme", new KategorijaOpreme
        {
            Naziv = "Nova kategorija",
            Opis = "Opis nove kategorije"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_ReturnsBadRequest_WhenInvalidModel()
    {
        var response = await _factory.CreateClient().PostAsJsonAsync("/api/kategorijaOpreme", new { });
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Put_UpdatesKategorija_ReturnsOk()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        ApiTestSeeders.ResetDatabase(db);
        var entity = new KategorijaOpreme { Naziv = "Stara kat", Opis = "Stari opis" };
        db.KategorijeOpreme.Add(entity);
        db.SaveChanges();

        entity.Naziv = "Azurirana kat";

        var response = await _factory.CreateClient().PutAsJsonAsync($"/api/kategorijaOpreme/{entity.Id}", entity);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Put_ReturnsNotFound_WhenNotExists()
    {
        var response = await _factory.CreateClient().PutAsJsonAsync("/api/kategorijaOpreme/99999", new KategorijaOpreme
        {
            Naziv = "X",
            Opis = "Y"
        });
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_HardDeletes_ReturnsOk()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        ApiTestSeeders.ResetDatabase(db);
        var entity = new KategorijaOpreme { Naziv = "Za brisanje", Opis = "Opis" };
        db.KategorijeOpreme.Add(entity);
        db.SaveChanges();

        var response = await _factory.CreateClient().DeleteAsync($"/api/kategorijaOpreme/{entity.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        using var verifyScope = _factory.Services.CreateScope();
        var verifyDb = verifyScope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
        var exists = await verifyDb.KategorijeOpreme.AnyAsync(x => x.Id == entity.Id);
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenNotExists()
    {
        var response = await _factory.CreateClient().DeleteAsync("/api/kategorijaOpreme/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
