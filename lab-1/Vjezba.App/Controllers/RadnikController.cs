using Microsoft.AspNetCore.Mvc;
using Vjezba.App.Repositories.EF;

namespace Vjezba.App.Controllers;

[Route("radnici")]
public class RadnikController : Controller
{
    private readonly EFRadnikRepository _repository;

    public RadnikController(EFRadnikRepository repository)
    {
        _repository = repository;
    }

    [Route("")]
    public IActionResult Index()
    {
        var items = _repository.GetAll();
        return View(items);
    }

    [Route("profil/{id:int}")]
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
