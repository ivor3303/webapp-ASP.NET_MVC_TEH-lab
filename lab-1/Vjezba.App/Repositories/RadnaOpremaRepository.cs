using Vjezba.Model;

namespace Vjezba.App.Repositories;

public class RadnaOpremaRepository
{
    public IReadOnlyList<RadnaOprema> GetAll()
    {
        return MockDataStore.RadnaOprema;
    }

    public RadnaOprema? GetById(int id)
    {
        return MockDataStore.RadnaOprema.FirstOrDefault(x => x.Id == id);
    }
}
