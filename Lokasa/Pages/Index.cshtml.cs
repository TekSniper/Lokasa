using Lokasa.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace Lokasa.Pages
{
    public class IndexModel : PageModel
    {
        public Models.Agent agent = new Models.Agent();
        public string SuccessMessage { get; set; }= string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string WarningMessage { get; set; } = string.Empty;
        public string EmptyField1 { get; set; } = string.Empty;
        public string EmptyField2 { get; set; } = string.Empty;
        public string LoginAgent { get; set; } = string.Empty;
        public string FonctionAgent { get; set; } = string.Empty;
        private readonly ILogger<IndexModel> _logger;
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public void OnGet()
        {
            LoginAgent = HttpContext.Session.GetString("Login")!;
            FonctionAgent = HttpContext.Session.GetString("Fonction")!;
            if(LoginAgent != null)
            {
                if(FonctionAgent.Contains("Directeur") || FonctionAgent.Contains("Admin"))
                {
                    Response.Redirect("/Admin/Dashboard");
                }
                else
                {
                    Response.Redirect("Presence/Presence");
                }
            }
            else
            {

            }
        }
        public void OnPost()
        {
            try
            {
                agent.Email = Request.Form["email"].ToString();
                agent.MotDePasse = Request.Form["pwd"].ToString();
                if(agent.Email.Length == 0)
                {
                    EmptyField1 = "Vueillez remplir votre identifiant ou email...";
                }
                else if(agent.MotDePasse.Length == 0)
                {
                    EmptyField2 = "Vueillez saisir votre mot de passe...";
                }
                else
                {
                    var isExists = agent.Exists();
                    switch (isExists)
                    {
                        case true:
                            {
                                agent.Etat = agent.GetEtat();
                                if (agent.Etat == "Actif")
                                {
                                    var isAuthenticated = agent.Authentification();
                                    switch (isAuthenticated)
                                    {
                                        case true:
                                            {
                                                var isDefaultPwd = agent.IsDefaultPassword("123456");
                                                if (isDefaultPwd)
                                                {
                                                    HttpContext.Session.SetString("Login", agent.Email);
                                                    HttpContext.Session.SetString("Fonction", agent.GetFonction());
                                                    Response.Redirect("/Agent/ChangerMotDePasse");
                                                }
                                                else
                                                {
                                                    HttpContext.Session.SetString("Login", agent.Email);
                                                    HttpContext.Session.SetString("Fonction", agent.GetFonction());
                                                    var fx = HttpContext.Session.GetString("Fonction")!;
                                                    var mot = "Directeur";
                                                    var mot2 = "Admin";
                                                    if (fx.Contains(mot) || fx.Contains(mot2))
                                                    {
                                                        Response.Redirect("/Admin/Dashboard");
                                                    }
                                                    else
                                                    {
                                                        Response.Redirect("/Presence/Presence");
                                                    }
                                                }                                                
                                            }
                                            break;
                                        case false:
                                            {
                                                ErrorMessage = "Echec lors de la connexion. Veuillez vérifier vos identifiants, le login et/ou le mot de passe...";
                                                agent.MotDePasse = string.Empty;
                                            }
                                            break;
                                    }
                                }
                                else
                                {
                                    WarningMessage = "Votre profil est inactif. Vueillez contacter l'Administrateur.";
                                    agent.MotDePasse = string.Empty;
                                }
                            }
                            break;
                        case false:
                            {
                                ErrorMessage = "Ce compte " + agent.Email + " n'existe pas !";
                                agent.MotDePasse = string.Empty;
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
