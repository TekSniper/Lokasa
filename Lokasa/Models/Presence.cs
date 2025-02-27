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
                var cm = new MySqlCommand("select * from presence where idagent=@agent and date_presence=@date and heure_arrivee is not null",cnx);
                cm.Parameters.AddWithValue("@agent",IdAgent);
                cm.Parameters.AddWithValue("@date",DatePresence);
                using(var reader = cm.ExecuteReader()){
                    if(reader.HasRows)
                        isTrue = true;
                    else
                        isTrue = false;
                }
            }

            return isTrue;
        }
    }
}
