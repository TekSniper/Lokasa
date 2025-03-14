using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lokasa.Pages.Tache
{
    public class TachesModel : PageModel
    {
        public string LoginAgent { get; set; } = string.Empty;
        public string FonctionAgent { get; set; } = string.Empty;
        public string WarningMessage { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string SuccessMessage { get; set; } = string.Empty;
        public Models.Agent agent = new Models.Agent();
        public Models.Tache tache = new Models.Tache();
        public List<Models.Tache> taches = new List<Models.Tache>();
        public void OnGet()
        {
            try
            {
                LoginAgent = HttpContext.Session.GetString("Login")!;
                FonctionAgent = HttpContext.Session.GetString("Fonction")!;

                if (HttpContext.Session.GetString("Login") == null)
                    Response.Redirect("/");
                else
                {
                    if (Request.Query["Date"].ToString() == null)
                    {
                        tache.DateDebut = DateTime.Now.Date;
                    }
                    else
                    {
                        tache.DateDebut = DateTime.Parse(Request.Query["Date"].ToString()).Date;
                    }
                    if (FonctionAgent == "Directeur")
                    {
                        switch (Request.Query["Email"].ToString() == null)
                        {
                            case true:
                                {
                                    using (var cnx = new DbConnexion().GetConnection())
                                    {
                                        cnx.Open();
                                        var cm = new MySqlCommand("select * from tache where date_debut=@ate", cnx);
                                        cm.Parameters.AddWithValue("@date", tache.DateDebut);
                                        using (var reader = cm.ExecuteReader())
                                        {
                                            while (reader.Read())
                                            {
                                                taches.Add(new Models.Tache
                                                {
                                                    Id = reader.GetInt64(0),
                                                    IdAgent = reader.GetInt32(1),
                                                    Titre = reader.GetString(2),
                                                    Description = reader.GetString(3),
                                                    DateDebut = reader.GetDateTime(4),
                                                    DateFin = reader.GetDateTime(5),
                                                    Etat = reader.GetString(6)
                                                });
                                            }
                                        }
                                    }
                                }
                                break;
                            case false:
                                {
                                    agent.Email = Request.Query["Email"].ToString();
                                    tache.IdAgent = agent.GetId();
                                    using (var cnx = new DbConnexion().GetConnection())
                                    {
                                        cnx.Open();
                                        var cm = new MySqlCommand("TaskAgentByDate", cnx);
                                        cm.CommandType = System.Data.CommandType.StoredProcedure;
                                        cm.Parameters.AddWithValue("vagent", tache.IdAgent);
                                        cm.Parameters.AddWithValue("vdate", tache.DateDebut);
                                        using (var reader = cm.ExecuteReader())
                                        {
                                            while (reader.Read())
                                            {
                                                taches.Add(new Models.Tache
                                                {
                                                    Id = reader.GetInt64(0),
                                                    IdAgent = reader.GetInt32(1),
                                                    Titre = reader.GetString(2),
                                                    Description = reader.GetString(3),
                                                    DateDebut = reader.GetDateTime(4),
                                                    DateFin = reader.GetDateTime(5),
                                                    Etat = reader.GetString(6)
                                                });
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        agent.Email = LoginAgent;
                        tache.IdAgent = agent.GetId();
                        using (var cnx = new DbConnexion().GetConnection())
                        {
                            cnx.Open();
                            var cm = new MySqlCommand("TaskAgentByDate", cnx);
                            cm.CommandType = System.Data.CommandType.StoredProcedure;
                            cm.Parameters.AddWithValue("vagent", tache.IdAgent);
                            cm.Parameters.AddWithValue("vdate", Convert.ToDateTime(Request.Query["date"]).Date);
                            using (var reader = cm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    taches.Add(new Models.Tache
                                    {
                                        Id = reader.GetInt64(0),
                                        IdAgent = reader.GetInt32(1),
                                        Titre = reader.GetString(2),
                                        Description = reader.GetString(3),
                                        DateDebut = reader.GetDateTime(4),
                                        DateFin = reader.GetDateTime(5),
                                        Etat = reader.GetString(6)
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

        }
    }
}
