using Lokasa.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lokasa.Pages.Agent
{
    public class DeconnexionModel : PageModel
    {
        public string LoginAgent { get; set; } = string.Empty;
        public string FonctionAgent { get; set; } = string.Empty;
        public void OnGet()
        {
            HttpContext.Session.Remove("Login");
            HttpContext.Session.Remove("Fonction");
            Response.Redirect("/");
        }
    }
}
