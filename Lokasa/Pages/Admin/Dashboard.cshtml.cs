using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lokasa.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        public string LoginAgent { get; set; } = string.Empty;
        public string FonctionAgent { get; set; } = string.Empty;
        public List<ChartView> EtatsView = new List<ChartView>();
        public List<Models.Tache> taches = new List<Models.Tache>();
        public void OnGet()
        {
            LoginAgent = HttpContext.Session.GetString("Login")!;
            FonctionAgent = HttpContext.Session.GetString("Fonction")!;
            if (HttpContext.Session.GetString("Login") == null)
                Response.Redirect("/");
            else
            {
                if (HttpContext.Session.GetString("Fonction")!.Contains("Directeur") || HttpContext.Session.GetString("Fonction")!.Contains("Admin") 
                    || HttpContext.Session.GetString("Fonction")!.Contains("Secr�taire"))
                {
                    using(var cnx = new DbConnexion().GetConnection())
                    {
                        cnx.Open();
                        using(var cm = new MySqlCommand("SELECT etat, COUNT(*) AS NombreTaches " +
                            "FROM tache WHERE WEEK(date_debut) = WEEK(CURDATE()) AND " +
                            "YEAR(date_debut) = YEAR(CURDATE()) GROUP BY etat", cnx))
                        {
                            using (var reader = cm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    EtatsView.Add(new ChartView
                                    {
                                        EtatTaches = reader.GetByte(0),
                                        NombreTaches = reader.GetInt64(1)
                                    });
                                }
                            }
                        }
                        using(var cm1 = new MySqlCommand("select * from tache order by id desc limit 10", cnx))
                        {
                            using(var reader = cm1.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    taches.Add(new Models.Tache
                                    {
                                        Id = reader.GetInt64(0),
                                        IdAgent = reader.GetInt32(1),
                                        Titre = reader.GetString(2),
                                        Description = reader.GetString(3),
                                        DateDebut = reader.GetDateTime(4).Date,
                                        DateFin = reader.GetDateTime(5).Date,
                                        Etat = reader.GetByte(7)
                                    });
                                }
                            }
                        }
                    }
                }
                else
                    Response.Redirect("/Presence/Presence");
            }
        }

        public long GetAgentsActifs()
        {
            long count = 0;
            using (var cnx = new DbConnexion().GetConnection())
            {
                cnx.Open();
                using (var cm = new MySqlCommand("select count(id) from agent where etat=@etat", cnx))
                {
                    cm.Parameters.AddWithValue("@etat", "Actif");
                    count = (long)cm.ExecuteScalar();
                }
            }
            return count;
        }

        public long GetAgentsPressents()
        {
            long count = 0;
            using (var cnx = new DbConnexion().GetConnection())
            {
                cnx.Open();
                using (var cm = new MySqlCommand("select count(idagent) from presence where date_presence=@date", cnx))
                {
                    cm.Parameters.AddWithValue("@date", DateTime.Now.Date);
                    count = (long)cm.ExecuteScalar();
                }
            }
            return count;
        }

        public long GetNumTaskHebdo()
        {
            long count = 0;
            using (var cnx = new DbConnexion().GetConnection())
            {
                cnx.Open();
                using (var cm = new MySqlCommand("SELECT COUNT(*) " +
                                                 "FROM tache WHERE WEEK(date_debut) = WEEK(CURDATE()) AND "+
                                                "YEAR(date_debut) = YEAR(CURDATE())", cnx))
                {
                    count = (long)cm.ExecuteScalar();
                }
            }
            return count;
        }
    }
}
