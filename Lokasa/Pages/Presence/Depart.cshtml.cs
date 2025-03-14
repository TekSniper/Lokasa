using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Lokasa.Models;

namespace Lokasa.Pages.Presence
{
    public class DepartModel : PageModel
    {
        public string LoginAgent { get; set; } = string.Empty;
        public string FonctionAgent { get; set; } = string.Empty;
        public string WarningMessage { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string SuccessMessage { get; set; } = string.Empty;
        public Models.Presence presence = new Models.Presence();
        public Models.Agent agent = new Models.Agent();
        public void OnGet()
        {
            try
            {
                LoginAgent = HttpContext.Session.GetString("Login")!;
                FonctionAgent = HttpContext.Session.GetString("Fonction")!;
                presence.IdAgent = agent.GetId();
                if (HttpContext.Session.GetString("Login") == null)
                    Response.Redirect("/");
                else
                {
                    presence.DatePresence = DateTime.Now.Date;
                    agent.Email = LoginAgent;
                    presence.IdAgent = agent.GetId();
                    var estPresent = presence.CheckPresence();
                    if (estPresent)
                    { 
                        var heure = presence.GetHeureArrivee();
                        SuccessMessage = "Vous êtes arrivé(e) à "+heure+".";
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
            try
            {
                agent.Email = HttpContext.Session.GetString("Login")!;
                presence.IdAgent = agent.GetId();
                presence.DatePresence = DateTime.Now.Date;
                presence.HeureDepart = TimeSpan.Parse(Request.Form["heure"].ToString());
                var estSorti = presence.DepartAgent();
                switch (estSorti)
                {
                    case true:
                        {
                            Response.Redirect("/Tache/Taches");
                        }
                        break;
                    case false:
                        {
                            ErrorMessage = "Erreur lors de l'enregistrement de votre départ";
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                WarningMessage = ex.Message;
            }
        }
    }
}
