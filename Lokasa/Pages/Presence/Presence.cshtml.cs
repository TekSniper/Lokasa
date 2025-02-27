using Lokasa.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Ocsp;

namespace Lokasa.Pages.Presence
{
    public class PresenceModel : PageModel
    {
        public string LoginAgent { get; set; } = string.Empty;
        public string FonctionAgent { get; set; } = string.Empty;
        public Models.Presence presence = new Models.Presence();
        public List<Models.Presence> presences = new List<Models.Presence>();
        public Agent agent = new Agent();
        public void OnGet()
        {
            LoginAgent = HttpContext.Session.GetString("Login")!;
            FonctionAgent = HttpContext.Session.GetString("Fonction")!;
            if (HttpContext.Session.GetString("Login") == null)
                Response.Redirect("/");
            else
            {
                if (HttpContext.Session.GetString("Fonction") == "Directeur")
                {
                    var vagent = Request.Query["ag"].ToString();
                    var mois = Request.Query["mo"].ToString();
                    var date_r = Request.Query["date"].ToString();
                    if(Request.Query["date"].ToString().Length == 0 && vagent.Length == 0)
                    {
                        using (var cnx = new DbConnexion().GetConnection())
                        {
                            cnx.Open();
                            var cm = new MySqlCommand("select * from presence where date_presence=@date", cnx);
                            cm.Parameters.AddWithValue("@date", DateTime.Now);
                            var dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                presences.Add(new Models.Presence
                                {
                                    Id = dr.GetInt64(0),
                                    IdAgent = dr.GetInt32(1),
                                    Jour = dr.GetString(2),
                                    DatePresence = dr.GetDateTime(3),
                                    HeureArrivee = dr.GetTimeSpan(4),
                                    HeureDepart = dr.GetTimeSpan(5)
                                });
                            }
                        }
                    }
                    else if(Request.Query["date"].ToString().Length == 0 && vagent.Length > 0)
                    {
                        using (var cnx = new DbConnexion().GetConnection())
                        {
                            cnx.Open();
                            var cm = new MySqlCommand("select * from presence where date_presence=@date and idagent=@agent", cnx);
                            agent.Email = vagent;
                            cm.Parameters.AddWithValue("@agent", agent.GetId());
                            cm.Parameters.AddWithValue("@date", DateTime.Now);
                            var dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                presences.Add(new Models.Presence
                                {
                                    Id = dr.GetInt64(0),
                                    IdAgent = dr.GetInt32(1),
                                    Jour = dr.GetString(2),
                                    DatePresence = dr.GetDateTime(3),
                                    HeureArrivee = dr.GetTimeSpan(4),
                                    HeureDepart = dr.GetTimeSpan(5)
                                });
                            }
                        }
                    }
                    else if(Request.Query["date"].ToString().Length > 0 && vagent.Length == 0){
                        using (var cnx = new DbConnexion().GetConnection())
                        {
                            cnx.Open();
                            var cm = new MySqlCommand("select * from presence where date_presence=@date", cnx);
                            cm.Parameters.AddWithValue("@date", DateTime.Parse(date_r));
                            var dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                presences.Add(new Models.Presence
                                {
                                    Id = dr.GetInt64(0),
                                    IdAgent = dr.GetInt32(1),
                                    Jour = dr.GetString(2),
                                    DatePresence = dr.GetDateTime(3),
                                    HeureArrivee = dr.GetTimeSpan(4),
                                    HeureDepart = dr.GetTimeSpan(5)
                                });
                            }
                        }
                    }
                    else{
                        using (var cnx = new DbConnexion().GetConnection())
                        {
                            cnx.Open();
                            var cm = new MySqlCommand("select * from presence where date_presence=@date and idagent=@agent", cnx);
                            agent.Email = vagent;
                            cm.Parameters.AddWithValue("@agent", agent.GetId());
                            cm.Parameters.AddWithValue("@date", DateTime.Parse(date_r));
                            var dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                presences.Add(new Models.Presence
                                {
                                    Id = dr.GetInt64(0),
                                    IdAgent = dr.GetInt32(1),
                                    Jour = dr.GetString(2),
                                    DatePresence = dr.GetDateTime(3),
                                    HeureArrivee = dr.GetTimeSpan(4),
                                    HeureDepart = dr.GetTimeSpan(5)
                                });
                            }
                        }
                    }
                    /*
                    if (vagent.Length == 0 && (mois.Length == 0 || mois == "present"))
                    {
                        using (var cnx = new DbConnexion().GetConnection())
                        {
                            cnx.Open();
                            var cm = new MySqlCommand("presences", cnx);
                            cm.CommandType = System.Data.CommandType.StoredProcedure;
                            cm.Parameters.AddWithValue("vdate", DateTime.Now);
                            var dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                presences.Add(new Models.Presence
                                {
                                    Id = dr.GetInt64(0),
                                    IdAgent = dr.GetInt32(1),
                                    Jour = dr.GetString(2),
                                    DatePresence = dr.GetDateTime(3),
                                    HeureArrivee = dr.GetTimeSpan(4),
                                    HeureDepart = dr.GetTimeSpan(5)
                                });
                            }
                        }
                    }
                    else if (vagent.Length > 0 && (mois.Length == 0 || mois == "present"))
                    {
                        using (var cnx = new DbConnexion().GetConnection())
                        {
                            cnx.Open();
                            var cm = new MySqlCommand("presences_par_agent", cnx);
                            cm.CommandType = System.Data.CommandType.StoredProcedure;
                            cm.Parameters.AddWithValue("vdate", DateTime.Now);
                            cm.Parameters.AddWithValue("vagent", vagent);
                            var dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                presences.Add(new Models.Presence
                                {
                                    Id = dr.GetInt64(0),
                                    IdAgent = dr.GetInt32(1),
                                    Jour = dr.GetString(2),
                                    DatePresence = dr.GetDateTime(3),
                                    HeureArrivee = dr.GetTimeSpan(4),
                                    HeureDepart = dr.GetTimeSpan(5)
                                });
                            }
                        }
                    }
                    else if (vagent.Length == 0 && mois == "passé")
                    {
                        using (var cnx = new DbConnexion().GetConnection())
                        {
                            cnx.Open();
                            var cm = new MySqlCommand("presences_mois_passe", cnx);
                            cm.CommandType = System.Data.CommandType.StoredProcedure;
                            cm.Parameters.AddWithValue("vdate", DateTime.Now);
                            var dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                presences.Add(new Models.Presence
                                {
                                    Id = dr.GetInt64(0),
                                    IdAgent = dr.GetInt32(1),
                                    Jour = dr.GetString(2),
                                    DatePresence = dr.GetDateTime(3),
                                    HeureArrivee = dr.GetTimeSpan(4),
                                    HeureDepart = dr.GetTimeSpan(5)
                                });
                            }
                        }
                    }
                    else
                    {
                        using (var cnx = new DbConnexion().GetConnection())
                        {
                            cnx.Open();
                            var cm = new MySqlCommand("presences_par_agent_mois_passe", cnx);
                            cm.CommandType = System.Data.CommandType.StoredProcedure;
                            cm.Parameters.AddWithValue("vdate", DateTime.Now);
                            cm.Parameters.AddWithValue("vagent", vagent);
                            var dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                presences.Add(new Models.Presence
                                {
                                    Id = dr.GetInt64(0),
                                    IdAgent = dr.GetInt32(1),
                                    Jour = dr.GetString(2),
                                    DatePresence = dr.GetDateTime(3),
                                    HeureArrivee = dr.GetTimeSpan(4),
                                    HeureDepart = dr.GetTimeSpan(5)
                                });
                            }
                        }
                    }*/
                }
                else
                {
                    Response.Redirect("/Tache/Taches");
                }
            }
        }
    }
}
