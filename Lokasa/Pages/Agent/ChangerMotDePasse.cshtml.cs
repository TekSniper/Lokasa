using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lokasa.Pages.Agent
{
    public class ChangerMotDePasseModel : PageModel
    {
        public string SuccessMessage { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string WarningMessage { get; set; } = string.Empty;
        public string ConfirmPwd { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string LoginAgent { get; set; } = string.Empty;
        public Models.Agent agent = new Models.Agent();
        public void OnGet()
        {
            LoginAgent = HttpContext.Session.GetString("Login")!;
            if (LoginAgent == null)
                Response.Redirect("/");
            else { }
        }
        public void OnPost()
        {
            try
            {
                agent.Email = HttpContext.Session.GetString("Login")!;
                agent.MotDePasse = Request.Form["pwd"].ToString();
                var confPwd = Request.Form["confpwd"].ToString();
                if(agent.Email.Length == 0 || agent.MotDePasse.Length == 0)
                {
                    WarningMessage = "Remplissez le mot de passe s'il vous plait !";
                    LoginAgent = HttpContext.Session.GetString("Login")!;
                    return;
                }
                else
                {
                    if(agent.MotDePasse != confPwd)
                    {
                        ConfirmPwd = "Les mots de passe ne sont pas identiques !";
                        LoginAgent = HttpContext.Session.GetString("Login")!;
                        return;
                    }
                    else
                    {
                        var isUpdated = agent.UpdatePwd();
                        switch(isUpdated)
                        {
                            case true:
                                {
                                    Response.Redirect("/Admin/Dashboard");
                                }
                                break;
                            case false:
                                {
                                    ErrorMessage = "Echec lors de la modification de votre mot de passe !";
                                }
                                break;
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
