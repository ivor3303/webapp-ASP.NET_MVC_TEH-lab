using Microsoft.AspNetCore.Mvc;
using Vjezba.App.Repositories.EF;

namespace Vjezba.App.Controllers;

[Route("odrzavanja")]
public class OdrzavanjeController : Controller
{
    private readonly EFOdrzavanjeRepository _repository;

    public OdrzavanjeController(EFOdrzavanjeRepository repository)
    {
        _repository = repository;
    }

    [Route("")]
    public IActionResult Index()
    {
        var items = _repository.GetAll();
        return View(items);
    }

    [Route("zapis/{id:int}")]
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
