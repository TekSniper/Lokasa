using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lokasa.Pages.Admin
{
    public class DashboardModel : PageModel
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
                if (HttpContext.Session.GetString("Fonction") == "Directeur")
                { }
                else
                    Response.Redirect("/Presence/Presence");
            }
        }
    }
}
