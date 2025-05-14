
namespace Lokasa.Repositories
{
    public class FonctionRepository : IFonctionRepository
    {
        public async Task<IEnumerable<Fonction>> GetAllFonctions()
        {
            using(var db = AppConnection.GetConnection())
            {
                await db.OpenAsync();
                var query = "select * from fonction";
                return await db.QueryAsync<Fonction>(query);
            }
        }
        public async Task<bool> Create(Fonction fonction)
        {
            using (var db = AppConnection.GetConnection())
            {
                await db.OpenAsync();
                var query = "insert into fonction (designation) values (@Designation)";
                var result = await db.ExecuteAsync(query, new { Designation = fonction.Designation });
                return result != 0;
            }
        }
    }
}
