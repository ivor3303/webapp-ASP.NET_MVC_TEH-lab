using Microsoft.AspNetCore.Mvc;
using Vjezba.App.Repositories.EF;

namespace Vjezba.App.Controllers;

public class ZaduzenjeOpremeController : Controller
{
    private readonly EFZaduzenjeOpremeRepository _repository;

    public ZaduzenjeOpremeController(EFZaduzenjeOpremeRepository repository)
    {
        _repository = repository;
    }

    public IActionResult Index()
    {
        var items = _repository.GetAll();
        return View(items);
    }

    public IActionResult Details(int id)
    {
        var item = _repository.GetById(id);
        if (item is null)
        {
            return NotFound();
        }

        return View(item);
    }
}
