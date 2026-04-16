using Microsoft.AspNetCore.Mvc;
using Vjezba.App.Repositories;

namespace Vjezba.App.Controllers;

public class ZaduzenjeOpremeController : Controller
{
    private readonly ZaduzenjeOpremeRepository _repository;

    public ZaduzenjeOpremeController(ZaduzenjeOpremeRepository repository)
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
