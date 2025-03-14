using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Lokasa.Models;
using Google.Protobuf.WellKnownTypes;

namespace Lokasa.Pages.Presence
{
    public class ArriveeModel : PageModel
    {
        public string LoginAgent { get; set; } = string.Empty;
        public string FonctionAgent { get; set; } = string.Empty;
        public string WarningMessage { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public Models.Presence presence = new Models.Presence();
        public Models.Agent agent = new Models.Agent();
        public void OnGet()
        {
            LoginAgent = HttpContext.Session.GetString("Login")!;
            FonctionAgent = HttpContext.Session.GetString("Fonction")!;
            agent.Email = LoginAgent;
            presence.IdAgent = agent.GetId();
            if (HttpContext.Session.GetString("Login") == null)
                Response.Redirect("/");
            else
            {
                presence.DatePresence = DateTime.Now.Date;
                presence.IdAgent = agent.GetId();
                var estPresent = presence.CheckPresence();
                if (estPresent)
                    WarningMessage = "Vous avez déjà marqué votre présence pour aujourd'hui";
                else
                {

                }
            }
        }
        public void OnPost()
        {
            try
            {
                agent.Email = HttpContext.Session.GetString("Login")!;
                presence.IdAgent = agent.GetId();
                presence.Jour = DateTime.Now.DayOfWeek.ToString();
                presence.DatePresence = DateTime.Now;
                presence.HeureArrivee = TimeSpan.Parse(Request.Form["heure"].ToString());
                var isCreated = presence.Create();
                switch (isCreated)
                {
                    case true:
                        {
                            Response.Redirect("/Tache/Taches");
                        }
                        break;
                    case false:
                        {
                            ErrorMessage = "Erreur lors de l'enregistrement de votre présence";
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
