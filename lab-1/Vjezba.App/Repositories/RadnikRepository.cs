using Vjezba.Model;

namespace Vjezba.App.Repositories;

public class RadnikRepository
{
    public IReadOnlyList<Radnik> GetAll()
    {
        return MockDataStore.Radnici;
    }

    public Radnik? GetById(int id)
    {
        return MockDataStore.Radnici.FirstOrDefault(x => x.Id == id);
    }
}
