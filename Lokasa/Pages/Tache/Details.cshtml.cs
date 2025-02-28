using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Lokasa.Models;
using MySql.Data.MySqlClient;

namespace Lokasa.Pages.Tache
{
    public class DetailsModel : PageModel
    {
        public string LoginAgent { get; set; } = string.Empty;
        public string FonctionAgent { get; set; } = string.Empty;
        public string WarningMessage { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string SuccessMessage { get; set; } = string.Empty;
        public Models.Presence presence = new Models.Presence();
        public Agent agent = new Agent();
        public Models.Tache tache = new Models.Tache();
        public List<Models.Tache> taches = new List<Models.Tache>();
        public void OnGet()
        {
            try
            {
                LoginAgent = HttpContext.Session.GetString("Login")!;
                FonctionAgent = HttpContext.Session.GetString("Fonction")!;
                agent.Email = LoginAgent;
                presence.IdAgent = agent.GetId();
                if (HttpContext.Session.GetString("Login") == null)
                    Response.Redirect("/");
                else
                {
                    presence.DatePresence = DateTime.Now;
                    presence.IdAgent = agent.GetId();
                    var estPresent = presence.CheckPresence();
                    if (estPresent)
                    {
                        tache.Id = int.Parse(Request.Query["ID"].ToString());
                        using (var cnx = new DbConnexion().GetConnection())
                        {
                            cnx.Open();
                            var cm = new MySqlCommand("select * from tache where id = @id", cnx);
                            cm.Parameters.AddWithValue("@id", tache.Id);
                            var dr = cm.ExecuteReader();
                            if (dr.Read())
                            {
                                tache.IdAgent = dr.GetInt32(1);
                                tache.Titre = dr.GetString(2);
                                tache.Description = dr.GetString(3);
                                tache.DateDebut = dr.GetDateTime(4);
                                tache.DateFin = dr.GetDateTime(5);
                                tache.Etat = dr.GetString(6);
                                tache.Commentaire = dr.GetString(7);
                            }
                        }
                    }
                    else
                    {
                        WarningMessage = "Vous n'avez pas marqué votre présence pour aujourd'hui";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
        public void OnPost()
        {

        }
    }
}
