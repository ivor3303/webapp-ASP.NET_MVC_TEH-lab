using Microsoft.AspNetCore.Mvc;
using Vjezba.App.Data;
using Vjezba.App.Repositories.EF;
using Vjezba.Model;

namespace Vjezba.App.Controllers;

[Route("izvjestaji")]
public class IzvjestajController : Controller
{
    private readonly EFIzvjestajRepository _repository;
    private readonly VjezbaDbContext _context;

    public IzvjestajController(EFIzvjestajRepository repository, VjezbaDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    [Route("")]
    public IActionResult Index()
    {
        var items = _repository.GetAll();
        return View(items);
    }

    [Route("detalji/{id:int}")]
    public IActionResult Details(int id)
    {
        var item = _repository.GetById(id);
        if (item is null)
        {
            return NotFound();
        }

        return View(item);
    }

    [HttpGet]
    [Route("create")]
    public IActionResult Create()
    {
        return View(new Izvjestaj { DatumIzrade = DateTime.Today });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("create")]
    public IActionResult Create(Izvjestaj model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _context.Izvjestaji.Add(model);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}