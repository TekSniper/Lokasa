using Lokasa.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;
using System.Text;

namespace Lokasa.Pages
{
    public class IndexModel : PageModel
    {
        public Agent agent = new Agent();
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
        private string EncryptPassword(string stringToEncrypt)
        {
            var pwd_encrypted = string.Empty;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(stringToEncrypt));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                pwd_encrypted = builder.ToString();
            }

            return pwd_encrypted;
        }
        public void OnGet()
        {
            LoginAgent = HttpContext.Session.GetString("Login")!;
            FonctionAgent = HttpContext.Session.GetString("Fonction")!;
            if(LoginAgent != null)
            {
                if(FonctionAgent == "Directeur")
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
                agent.MotDePasse = EncryptPassword(Request.Form["pwd"].ToString());
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
                                                HttpContext.Session.SetString("Login", agent.Email);
                                                HttpContext.Session.SetString("Fonction", agent.GetFonction());
                                                var fx = HttpContext.Session.GetString("Fonction");
                                                if (fx == "Directeur")
                                                {
                                                    Response.Redirect("/Admin/Dashboard");
                                                }
                                                else
                                                {
                                                    Response.Redirect("/Presence/Presence");
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
