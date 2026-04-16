using Vjezba.Model;

namespace Vjezba.App.Repositories;

public class ZaduzenjeOpremeRepository
{
    public IReadOnlyList<ZaduzenjeOpreme> GetAll()
    {
        return MockDataStore.Zaduzenja;
    }

    public ZaduzenjeOpreme? GetById(int id)
    {
        return MockDataStore.Zaduzenja.FirstOrDefault(x => x.Id == id);
    }
}
