using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lokasa.Pages.Agent
{
    public class LesAgentsModel : PageModel
    {
        public string LoginAgent { get; set; } = string.Empty;
        public string FonctionAgent { get; set; } = string.Empty;
        public string WarningMessage { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string SuccessMessage { get; set; } = string.Empty;
        public Models.Agent agent = new Models.Agent();
        public List<Models.Agent> agents = new List<Models.Agent>();
        public void OnGet()
        {
            LoginAgent = HttpContext.Session.GetString("Login")!;
            FonctionAgent = HttpContext.Session.GetString("Fonction")!;
            if(LoginAgent != null)
            {
                if(FonctionAgent.Contains("Directeur") || FonctionAgent.Contains("Admin"))
                {
                    switch(Request.Query["manlog"].ToString() == "all" || Request.Query["manlog"].ToString() == "")
                    {
                        case true:
                            {
                                using (var cnx = new DbConnexion().GetConnection())
                                {
                                    cnx.Open();
                                    var cm = new MySqlCommand("select * from agent", cnx);
                                    var reader = cm.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        var ag = new Models.Agent();
                                        ag.Id = reader.GetInt32("id");
                                        ag.IdService = reader.GetInt16("idservice");
                                        if (!reader.IsDBNull(2))
                                            ag.Matricule = reader.GetString("matricule");
                                        else
                                            ag.Matricule = "NULL";
                                        ag.Nom = reader.GetString("nom");
                                        ag.Prenom = reader.GetString("prenom").ToUpper();
                                        ag.Genre = reader.GetString("genre");
                                        ag.Email = reader.GetString("email");
                                        ag.Fonction = reader.GetString("fonction");
                                        ag.Etat = reader.GetString("etat");

                                        agents.Add(ag);
                                    }
                                }
                            }
                            break;
                        case false:
                            {
                                using (var cnx = new DbConnexion().GetConnection())
                                {
                                    cnx.Open();
                                    var cm = new MySqlCommand("select * from agent where matricule=@matricule or nom=@nom or email=@email", cnx);
                                    var parmAgent = Request.Query["manlog"].ToString();
                                    cm.Parameters.AddWithValue("@matricule", parmAgent);
                                    cm.Parameters.AddWithValue("@nom", parmAgent);
                                    cm.Parameters.AddWithValue("@email", parmAgent);
                                    var reader = cm.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        var ag = new Models.Agent();
                                        ag.Id = reader.GetInt32("id");
                                        ag.IdService = reader.GetInt16("idservice");
                                        if (reader.IsDBNull(2))
                                            ag.Matricule = "NULL";
                                        else
                                            ag.Matricule = reader.GetString("matricule");
                                        ag.Nom = reader.GetString("nom");
                                        ag.Prenom = reader.GetString("prenom").ToUpper();
                                        ag.Genre = reader.GetString("genre");
                                        ag.Email = reader.GetString("email");
                                        ag.Fonction = reader.GetString("fonction");
                                        ag.Etat = reader.GetString("etat");

                                        agents.Add(ag);
                                    }
                                }
                            }
                            break;
                    }
                }
                else
                {
                    Response.Redirect("/Tache/Taches");
                }
            }
            else
            {
                Response.Redirect("/");
            }
        }
    }
}
