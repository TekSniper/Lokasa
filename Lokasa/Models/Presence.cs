using MySql.Data.MySqlClient;

namespace Lokasa.Models
{
    public class Presence
    {
        public long Id { get; set; }
        public int IdAgent { get; set; }
        public string Jour { get; set; } = string.Empty;
        public DateTime DatePresence { get; set; }
        public TimeSpan HeureArrivee { get; set; }
        public TimeSpan HeureDepart { get; set; }
        public bool Create()
        {
            var isTrue = false;
            using(var cnx = new DbConnexion().GetConnection())
            {
                cnx.Open();
                var cm = new MySqlCommand("CreerPresence",cnx);
                cm.CommandType = System.Data.CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("vagent", IdAgent);
                cm.Parameters.AddWithValue("vjour", Jour);
                cm.Parameters.AddWithValue("vdate", DatePresence);
                cm.Parameters.AddWithValue("vheurea", HeureArrivee);
                var i = cm.ExecuteNonQuery();
                if(i != 0) 
                    isTrue = true;
                else
                    isTrue = false;
            }

            return isTrue;
        }
        public bool DepartAgent()
        {
            var isTrue = false;
            using(var cnx = new DbConnexion().GetConnection())
            {
                cnx.Open();
                var cm = new MySqlCommand("DepartAgent", cnx);
                cm.CommandType = System.Data.CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("vagent", IdAgent);
                cm.Parameters.AddWithValue("vheured",HeureDepart);
                cm.Parameters.AddWithValue("vdate",DatePresence);
                var i = cm.ExecuteNonQuery();
                if(i != 0)
                    isTrue = true; 
                else
                    isTrue = false;
            }

            return isTrue;
        }
        public bool CheckPresence() {
            var isTrue = false;
            using(var cnx = new DbConnexion().GetConnection()) {
                cnx.Open();
                var cm = new MySqlCommand("select count(*) from presence where idagent=@agent and date_presence=@date and heure_arrivee is not null",cnx);
                cm.Parameters.AddWithValue("@agent",IdAgent);
                cm.Parameters.AddWithValue("@date",DatePresence);
                var reader = cm.ExecuteReader();
                if(reader.Read())
                {
                    if (reader.GetInt64(0) == 0)
                        isTrue = false;
                    else
                        isTrue = true;
                }
            }

            return isTrue;
        }
        public bool CheckDepart()
        {
            var isTrue = false;
            using(var cnx = new DbConnexion().GetConnection())
            {
                cnx.Open();
                var cm = new MySqlCommand("select count(*) from presence where idagent=@agent and date_presence=@date and heur_depart is not null", cnx);
                cm.Parameters.AddWithValue("@agent", IdAgent);
                cm.Parameters.AddWithValue("@date", DatePresence);
                var reader = cm.ExecuteReader();
                if (reader.Read()) 
                {
                    if(reader.GetInt64(0) == 0)
                        isTrue = false;
                    else
                        isTrue = true;
                }
            }

            return isTrue;
        }
        public TimeSpan GetHeureArrivee(){
            var heure= DateTime.Now.TimeOfDay;
            using(var cnx = new DbConnexion().GetConnection()){
                cnx.Open();
                var cm = new MySqlCommand("SELECT p.heure_arrivee FROM presence p WHERE p.date_presence=@date AND p.idagent=@agent", cnx);
                cm.Parameters.AddWithValue("@agent",IdAgent);
                cm.Parameters.AddWithValue("@date",DatePresence);
                var reader = cm.ExecuteReader();
                if(reader.Read())
                    heure = reader.GetTimeSpan(0);
            }
            return heure;
        }
    }
}
