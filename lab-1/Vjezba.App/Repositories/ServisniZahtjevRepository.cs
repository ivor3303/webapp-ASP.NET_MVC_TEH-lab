using Vjezba.Model;

namespace Vjezba.App.Repositories;

public class ServisniZahtjevRepository
{
    public IReadOnlyList<ServisniZahtjev> GetAll()
    {
        return MockDataStore.ServisniZahtjevi;
    }

    public ServisniZahtjev? GetById(int id)
    {
        return MockDataStore.ServisniZahtjevi.FirstOrDefault(x => x.Id == id);
    }
}
