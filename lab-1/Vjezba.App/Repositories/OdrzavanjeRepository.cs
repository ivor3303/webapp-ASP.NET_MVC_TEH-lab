using Vjezba.Model;

namespace Vjezba.App.Repositories;

public class OdrzavanjeRepository
{
    public IReadOnlyList<Odrzavanje> GetAll()
    {
        return MockDataStore.Odrzavanja;
    }

    public Odrzavanje? GetById(int id)
    {
        return MockDataStore.Odrzavanja.FirstOrDefault(x => x.Id == id);
    }
}
