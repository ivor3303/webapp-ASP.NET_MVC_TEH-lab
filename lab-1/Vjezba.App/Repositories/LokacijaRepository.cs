using Vjezba.Model;

namespace Vjezba.App.Repositories;

public class LokacijaRepository
{
    public IReadOnlyList<Lokacija> GetAll()
    {
        return MockDataStore.Lokacije;
    }

    public Lokacija? GetById(int id)
    {
        return MockDataStore.Lokacije.FirstOrDefault(x => x.Id == id);
    }
}
