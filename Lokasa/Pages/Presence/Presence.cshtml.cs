using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lokasa.Pages.Presence
{
    public class PresenceModel : PageModel
    {
        public string LoginAgent { get; set; } = string.Empty;
        public string FonctionAgent { get; set; } = string.Empty;
        public void OnGet()
        {
            LoginAgent = HttpContext.Session.GetString("Login")!;
            FonctionAgent = HttpContext.Session.GetString("Fonction")!;
            if (HttpContext.Session.GetString("Login") == null)
                Response.Redirect("/");
            else
            {

            }
        }
    }
}
