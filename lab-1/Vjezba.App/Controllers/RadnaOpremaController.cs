using Microsoft.AspNetCore.Mvc;
using Vjezba.App.Repositories.EF;

namespace Vjezba.App.Controllers;

[Route("oprema")]
public class RadnaOpremaController : Controller
{
    private readonly EFRadnaOpremaRepository _repository;

    public RadnaOpremaController(EFRadnaOpremaRepository repository)
    {
        _repository = repository;
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
}
