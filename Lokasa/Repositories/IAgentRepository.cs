namespace Lokasa.Repositories
{
    public interface IAgentRepository
    {
        Task<bool> Create(Agent agent);
        Task<bool> UpdatePwd(string email, string password);
        Task<bool> ChangeStatus(string etat, string email);
        Task<int> GetId(string email);
        Task<string> GetEtat(string email);
        Task<string> GetFonction(string email);
        Task<bool> Authentification(string email, string mot_de_passe);
        Task<bool> IsExist(string email);
        Task<bool> IsDefaultPassword(string email, string mot_de_passe);
        Task<string> GetFullName(int id);
    }
}
