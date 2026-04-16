using Vjezba.Model;

namespace Vjezba.App.Repositories;

public class ProizvodacRepository
{
    public IReadOnlyList<Proizvodac> GetAll()
    {
        return MockDataStore.Proizvodaci;
    }

    public Proizvodac? GetById(int id)
    {
        return MockDataStore.Proizvodaci.FirstOrDefault(x => x.Id == id);
    }
}
