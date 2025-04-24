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
        public Models.Agent agent { get; set; } = new Models.Agent();
        public Models.Tache tache = new Models.Tache();
        public List<Models.Tache> taches = new List<Models.Tache>();
        public string AgentFullName = string.Empty;
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
                    var estAutorise = FonctionAgent.Contains("Directeur") || FonctionAgent.Contains("Admin");
                    if (estAutorise)
                    {
                        agent.Id = int.Parse(Request.Query["Agent"].ToString());
                        AgentFullName = agent.GetFullName();
                        tache.Id = int.Parse(Request.Query["ID"].ToString());
                        using (var cnx = new DbConnexion().GetConnection())
                        {
                            cnx.Open();
                            var cm = new MySqlCommand("select * from tache where id = @id", cnx);
                            cm.Parameters.AddWithValue("@id", tache.Id);
                            var dr = cm.ExecuteReader();
                            if (dr.Read())
                            {
                                tache.IdAgent = dr.GetInt32("idagent");
                                tache.Titre = dr.GetString("titre");
                                tache.Description = dr.GetString("description");
                                tache.DateDebut = dr.GetDateTime("date_debut").Date;
                                tache.DateFin = dr.GetDateTime("date_fin").Date;
                                tache.Etat = dr.GetByte("etat");
                                tache.Commentaire = dr.GetString("commentaire");
                            }
                        }
                    }
                    else
                    {
                        Response.Redirect("/Tache/Taches");
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
            try
            {
                tache.Id = long.Parse(Request.Form["id"].ToString());
                tache.IdAgent = int.Parse(Request.Form["idAgent"].ToString());
                tache.Titre = Request.Form["titre"].ToString();
                tache.Description = Request.Form["description"].ToString();
                tache.DateDebut = Convert.ToDateTime(Request.Form["dateDebut"]).Date;
                tache.DateFin = Convert.ToDateTime(Request.Form["dateFin"]).Date;
                tache.Etat = byte.Parse(Request.Form["etat"].ToString());
                tache.Commentaire = Request.Form["commentaire"].ToString();
                if(tache.Titre.Length == 0 || tache.Description.Length == 0 || tache.DateDebut == DateTime.MinValue || tache.DateFin == DateTime.MinValue)
                {
                    WarningMessage = "Remplissez les champs obligatoires s'il vous plait !";
                    LoginAgent = HttpContext.Session.GetString("Login")!;
                    FonctionAgent = HttpContext.Session.GetString("Fonction")!;
                }
                else
                {
                    var isUpdated = tache.UpdateTask();
                    switch(isUpdated)
                    {
                        case true:
                            {
                                Response.Redirect("/Tache/Taches");
                            }
                            break;
                        case false:
                            {
                                ErrorMessage = "Echec lors de la mise à jour des infos sur la tache";
                                LoginAgent = HttpContext.Session.GetString("Login")!;
                                FonctionAgent = HttpContext.Session.GetString("Fonction")!;
                            }
                            break;
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
