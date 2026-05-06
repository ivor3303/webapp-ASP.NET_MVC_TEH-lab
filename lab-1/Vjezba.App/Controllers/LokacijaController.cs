using Microsoft.AspNetCore.Mvc;
using Vjezba.App.Repositories.EF;

namespace Vjezba.App.Controllers;

[Route("lokacije")]
public class LokacijaController : Controller
{
    private readonly EFLokacijaRepository _repository;

    public LokacijaController(EFLokacijaRepository repository)
    {
        _repository = repository;
    }

    [Route("")]
    public IActionResult Index()
    {
        var items = _repository.GetAll();
        return View(items);
    }

    [Route("lokacija/{id:int}")]
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
