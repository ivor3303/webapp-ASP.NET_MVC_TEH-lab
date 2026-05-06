using Microsoft.AspNetCore.Mvc;
using Vjezba.App.Repositories.EF;

namespace Vjezba.App.Controllers;

public class ServisniZahtjevController : Controller
{
    private readonly EFServisniZahtjevRepository _repository;

    public ServisniZahtjevController(EFServisniZahtjevRepository repository)
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
