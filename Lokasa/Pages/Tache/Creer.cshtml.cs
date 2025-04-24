using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lokasa.Pages.Tache
{
    public class CreerModel : PageModel
    {
        public string LoginAgent { get; set; } = string.Empty;
        public string FonctionAgent { get; set; } = string.Empty;
        public string WarningMessage { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string SuccessMessage { get; set; } = string.Empty;
        public Models.Presence presence = new Models.Presence();
        public Models.Agent agent = new Models.Agent();
        public Models.Tache tache = new Models.Tache();
        public List<EtatTache> etats = new List<EtatTache>();
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
                { }
                else
                {
                    WarningMessage = "Vous n'avez pas marqué votre présence pour aujourd'hui";
                }
            }
        }

        public List<EtatTache> GetEtats()
        {
            using (var cnx = new DbConnexion().GetConnection())
            {
                cnx.Open();
                using (var cm = new MySqlCommand("select * from etat order by id asc",cnx))
                {
                    using (var reader = cm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            etats.Add(new EtatTache
                            {
                                Id = reader.GetByte("id"),
                                Designation = reader.GetString("designation")
                            });
                        }
                    }
                }
            }
            return etats;
        }
        public void OnPost()
        {
            try
            {
                agent.Email = HttpContext.Session.GetString("Login")!;
                tache.IdAgent = agent.GetId();
                tache.Titre = Request.Form["titre"].ToString();
                tache.Description = Request.Form["description"].ToString();
                tache.DateDebut = DateTime.Parse(Request.Form["dateDebut"].ToString());
                tache.DateFin = DateTime.Parse(Request.Form["dateFin"].ToString());
                tache.Etat = byte.Parse(Request.Form["etat"].ToString()) ;
                tache.Commentaire = Request.Form["commentaire"].ToString();
                if (tache.Titre == string.Empty || tache.Description == string.Empty || tache.DateDebut == DateTime.MinValue || tache.DateFin == DateTime.MinValue)
                {
                    ErrorMessage = "Les champs titre, description, les dates et l'état sont obligatoires";
                }
                else
                {
                    var isCreated = tache.Create();
                    switch (isCreated)
                    {
                        case true:
                            {
                                SuccessMessage = "Tâche créée avec succès";

                                tache.IdAgent = 0;
                                tache.Titre = string.Empty;
                                tache.Description = string.Empty;
                                tache.Commentaire = string.Empty;
                                LoginAgent = HttpContext.Session.GetString("Login")!;
                                FonctionAgent = HttpContext.Session.GetString("Fonction")!;
                                Response.Redirect("/Tache/Taches");
                            }
                            break;
                        case false:
                            {
                                ErrorMessage = "Erreur lors de la création de la tâche";
                                LoginAgent = HttpContext.Session.GetString("Login")!;
                                FonctionAgent = HttpContext.Session.GetString("Fonction")!;
                            }
                            break;
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
