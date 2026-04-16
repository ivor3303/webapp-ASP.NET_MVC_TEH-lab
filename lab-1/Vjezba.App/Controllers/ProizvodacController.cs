using Microsoft.AspNetCore.Mvc;
using Vjezba.App.Repositories;
using Vjezba.App.ViewModels;

namespace Vjezba.App.Controllers;

public class ProizvodacController : Controller
{
    private readonly ProizvodacRepository _repository;
    private readonly RadnaOpremaRepository _radnaOpremaRepository;

    public ProizvodacController(ProizvodacRepository repository, RadnaOpremaRepository radnaOpremaRepository)
    {
        _repository = repository;
        _radnaOpremaRepository = radnaOpremaRepository;
    }

    public IActionResult Index()
    {
        var items = _repository.GetAll();
        return View(items);
    }

    public IActionResult Details(int id)
    {
        var proizvodac = _repository.GetById(id);
        if (proizvodac is null)
        {
            return NotFound();
        }

        var model = new ProizvodacDetailsViewModel
        {
            Proizvodac = proizvodac,
            Oprema = _radnaOpremaRepository
                .GetAll()
                .Where(o => o.Proizvodac.Id == proizvodac.Id)
                .ToList()
        };

        return View(model);
    }
}
