using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lokasa.Pages.Agent
{
    public class CreerAgentModel : PageModel
    {
        public string LoginAgent { get; set; } = string.Empty;
        public string FonctionAgent { get; set; } = string.Empty;
        public string SuccessMessage { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string WarningMessage { get; set; } = string.Empty;
        public string ConfPwd { get; set;} = string.Empty;

        public Models.Agent agent = new Models.Agent();

        public List<Service> services = new List<Service>();

        public List<Fonction> fonctions = new List<Fonction>();
        public void OnGet()
        {
            LoginAgent = HttpContext.Session.GetString("Login")!;
            FonctionAgent = HttpContext.Session.GetString("Fonction")!;
            if (LoginAgent != null)
            {
                if (FonctionAgent != "Directeur")
                    Response.Redirect("/Tache/Taches");
                else 
                {

                }
            }
            else
                Response.Redirect("/");
        }
        public List<Service> GetServices()
        {
            using(var cnx = new DbConnexion().GetConnection())
            {
                cnx.Open();
                var cm = new MySqlCommand("select * from service", cnx);
                var reader = cm.ExecuteReader();
                while (reader.Read())
                {
                    services.Add(new Service
                    {
                        Id = reader.GetInt16("id"),
                        Designation = reader.GetString("designation")
                    });
                }
            }
            return services;
        }
        public List<Fonction> GetFonctions()
        {
            using(var cnx = new DbConnexion().GetConnection())
            {
                cnx.Open();
                var cm = new MySqlCommand("select * from fonction", cnx);
                var reader = cm.ExecuteReader();
                while (reader.Read())
                {
                    fonctions.Add(new Fonction
                    {
                        Id= reader.GetInt16("id"),
                        Designation = reader.GetString("designation")
                    });
                }
            }
            return fonctions;
        }
        public void OnPost()
        {
            try
            {
                agent.IdService = short.Parse(Request.Form["service"].ToString());
                agent.Matricule = Request.Form["matricule"].ToString();
                agent.Nom = Request.Form["nom"].ToString();
                agent.PostNom = Request.Form["postnom"].ToString();
                agent.Prenom = Request.Form["prenom"].ToString();
                agent.Genre = Request.Form["sexe"].ToString();
                agent.Email = Request.Form["email"].ToString();
                agent.MotDePasse = "123456";
                agent.Fonction = Request.Form["fonction"].ToString();
                agent.Etat = "Actif";
                if(agent.Matricule == string.Empty || agent.Nom == string.Empty || agent.Genre == string.Empty || agent.Email == string.Empty || 
                    agent.MotDePasse == string.Empty || agent.Fonction == string.Empty)
                {
                    WarningMessage = "Remplissez les champs obligatoires !";

                    LoginAgent = HttpContext.Session.GetString("Login")!;
                    FonctionAgent = HttpContext.Session.GetString("Fonction")!;
                    return;
                }
                else
                {
                    var isCreated = agent.Create();
                    if (isCreated)
                    {
                        SuccessMessage = "Compte créé avec succès !";
                        agent.Matricule = string.Empty;
                        agent.Nom = string.Empty;
                        agent.PostNom = string.Empty;
                        agent.Prenom = string.Empty;
                        agent.Genre = string.Empty;
                        agent.Email = string.Empty;
                        agent.MotDePasse = string.Empty;
                        agent.Fonction = string.Empty;
                        agent.Etat = string.Empty;
                        Response.Redirect("/Agent/LesAgents?manlog=all");
                    }
                    else
                    {
                        ErrorMessage = "Echec de la création du compte d'un agent";
                        LoginAgent = HttpContext.Session.GetString("Login")!;
                        FonctionAgent = HttpContext.Session.GetString("Fonction")!;
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
