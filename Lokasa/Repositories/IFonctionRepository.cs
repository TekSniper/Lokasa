namespace Lokasa.Repositories
{
    public interface IFonctionRepository
    {
        Task<IEnumerable<Fonction>> GetAllFonctions();
        Task<bool> Create(Fonction fonction);
    }
}
