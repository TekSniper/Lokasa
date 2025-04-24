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
        public byte Etat { get; set; } 
        public string Commentaire { get; set; } = string.Empty;
        private bool _isTrue = false;
        public bool Create()
        {
            using(var cnx = new DbConnexion().GetConnection())
            {
                cnx.Open();
                var cm = new MySqlCommand("CreerTache", cnx);
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
                    _isTrue = true;
                else
                    _isTrue = false;
            }

            return _isTrue;
        }
        public bool UpdateTask()
        {
            using(var cnx = new DbConnexion().GetConnection())
            {
                cnx.Open();
                var sql = "update tache set titre=@titre,description=@descri,date_debut=@dated,date_fin=@datef,etat=@etat,commentaire=@comment" +
                    "where id=@id";
                using(var cm = new MySqlCommand(sql, cnx))
                {
                    cm.Parameters.AddWithValue("@titre", this.Titre);
                    cm.Parameters.AddWithValue("@descri", this.Description);
                    cm.Parameters.AddWithValue("@dated", this.DateDebut);
                    cm.Parameters.AddWithValue("@datef", this.DateFin);
                    cm.Parameters.AddWithValue("@etat", this.Etat);
                    cm.Parameters.AddWithValue("@comment", this.Commentaire);

                    var i = cm.ExecuteNonQuery();
                    if (i != 0)
                        _isTrue = true;
                    else
                        _isTrue = false;
                }
            }
            return _isTrue;
        }
        public bool ChangeStatus()
        {
            using(var cnx = new DbConnexion().GetConnection())
            {
                cnx.Open();
                var cm = new MySqlCommand("UpdateEtatTache", cnx);
                cm.CommandType = System.Data.CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("vid", IdAgent);
                cm.Parameters.AddWithValue("vetat", Etat);

                var i = cm.ExecuteNonQuery();
                if(i != 0)
                    _isTrue = true; 
                else
                    _isTrue = false;
            }

            return _isTrue;
        }
    }
}
