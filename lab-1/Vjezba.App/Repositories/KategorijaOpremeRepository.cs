using Vjezba.Model;

namespace Vjezba.App.Repositories;

public class KategorijaOpremeRepository
{
    public IReadOnlyList<KategorijaOpreme> GetAll()
    {
        return MockDataStore.Kategorije;
    }

    public KategorijaOpreme? GetById(int id)
    {
        return MockDataStore.Kategorije.FirstOrDefault(x => x.Id == id);
    }
}
