using Microsoft.AspNetCore.Mvc;
using Vjezba.App.Repositories;

namespace Vjezba.App.Controllers;

public class ServisniZahtjevController : Controller
{
    private readonly ServisniZahtjevRepository _repository;

    public ServisniZahtjevController(ServisniZahtjevRepository repository)
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
