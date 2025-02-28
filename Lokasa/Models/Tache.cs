using MySql.Data.MySqlClient;

namespace Lokasa.Models
{
    public class Tache
    {
        public long Id { get; set; }
        public int IdAgent { get; set; }
        public string Titre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string Etat { get; set; } = string.Empty;
        public string Commentaire { get; set; } = string.Empty;
        public bool Create()
        {
            var isTrue = false;
            using(var cnx = new DbConnexion().GetConnection())
            {
                cnx.Open();
                var cm = new MySqlCommand("CreerPresence", cnx);
                cm.CommandType = System.Data.CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("vagent", IdAgent);
                cm.Parameters.AddWithValue("vtitre", Titre);
                cm.Parameters.AddWithValue("vdescription", Description);
                cm.Parameters.AddWithValue("vdatedebut", DateDebut);
                cm.Parameters.AddWithValue("vdatefin", DateFin);
                cm.Parameters.AddWithValue("vetat", Etat);
                cm.Parameters.AddWithValue("vcommentaire", Commentaire);
                var i = cm.ExecuteNonQuery();
                if(i != 0)
                    isTrue = true;
                else
                    isTrue = false;
            }

            return isTrue;
        }
        public bool ChangeStatus()
        {
            var isTrue = false ;
            using(var cnx = new DbConnexion().GetConnection())
            {
                cnx.Open();
                var cm = new MySqlCommand("UpdateEtatTache", cnx);
                cm.CommandType = System.Data.CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("vid", IdAgent);
                cm.Parameters.AddWithValue("vetat", Etat);

                var i = cm.ExecuteNonQuery();
                if(i != 0)
                    isTrue = true; 
                else
                    isTrue = false;
            }

            return isTrue;
        }
    }
}
