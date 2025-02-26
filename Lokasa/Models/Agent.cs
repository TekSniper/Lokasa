using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Lokasa.Models
{
    public class Agent
    {
        public int Id { get; set; }
        public short IdService { get; set; }
        public string Matricule { get; set; } = string.Empty;
        public string Nom { get; set; } = string.Empty;
        public string PostNom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MotDePasse { get; set; } = string.Empty;
        public string Fonction {  get; set; } = string.Empty;
        public string Etat {  get; set; } = string.Empty;
        
        public bool Create()
        {
            var isTrue = false;
            using(var cnx=new DbConnexion().GetConnection())
            {
                cnx.Open();
                var cm = new MySqlCommand("CreerAgent", cnx);
                cm.CommandType = System.Data.CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("vservice", this.IdService);
                cm.Parameters.AddWithValue("vmatricule", this.Matricule);
                cm.Parameters.AddWithValue("vnom", this.Nom);
                cm.Parameters.AddWithValue("vpostnom", this.PostNom);
                cm.Parameters.AddWithValue("vprenom", this.Prenom);
                cm.Parameters.AddWithValue("vgenre", this.Genre);
                cm.Parameters.AddWithValue("vlogin", this.Email);
                cm.Parameters.AddWithValue("vpwd", this.MotDePasse);
                cm.Parameters.AddWithValue("vfonction", this.Fonction);
                cm.Parameters.AddWithValue("vetat", this.Etat);
            }

            return isTrue;
        }
        public bool ChangeStatus()
        {
            var isTrue = false;
            using(var cnx = new DbConnexion().GetConnection())
            {
                cnx.Open();
                var cm = new MySqlCommand("ChangerEtat", cnx);
                cm.CommandType= System.Data.CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("vetat",this.Etat);
                cm.Parameters.AddWithValue("vlogin",this.Email);
            }

            return isTrue;
        }
        public int GetId()
        {
            var id = 0;
            using(var cnx = new DbConnexion().GetConnection())
            {
                cnx.Open();
                var cm = new MySqlCommand("select id from agent where email=@email", cnx);
                cm.Parameters.AddWithValue("@email", this.Email);
                var reader = cm.ExecuteReader();
                if(reader.Read())
                    id = reader.GetInt32(0);
            }

            return id;
        }
        public string GetEtat()
        {
            var etat = string.Empty;
            using (var cnx = new DbConnexion().GetConnection())
            {
                cnx.Open();
                var cm = new MySqlCommand("select etat from agent where email=@email", cnx);
                cm.Parameters.AddWithValue("@email", this.Email);
                var reader = cm.ExecuteReader();
                if (reader.Read())
                    etat = reader.GetString(0);
            }

            return etat;
        }
        public string GetFonction()
        {
            var fonction = string.Empty;
            using (var cnx = new DbConnexion().GetConnection())
            {
                cnx.Open();
                var cm = new MySqlCommand("select fontion from agent where email=@email", cnx);
                cm.Parameters.AddWithValue("@email", this.Email);
                var reader = cm.ExecuteReader();
                if (reader.Read())
                    fonction = reader.GetString(0);
            }

            return fonction;
        }
        public bool Authentification()
        {
            var isTrue = false;
            using (var cnx = new DbConnexion().GetConnection())
            {
                cnx.Open();
                var cm = new MySqlCommand("select * from agent where email=@email and mot_de_passe=@pwd", cnx);
                cm.Parameters.AddWithValue("@email", this.Email);
                cm.Parameters.AddWithValue("@pwd", this.MotDePasse);
                var reader = cm.ExecuteReader();
                if (reader.Read())
                    isTrue = true;
            }

            return isTrue;
        }
        public bool Exists()
        {
            var isTrue = false;
            using(var cnx = new DbConnexion().GetConnection())
            {
                cnx.Open();
                var cm = new MySqlCommand("select count(*) from agent where email=@email", cnx);
                cm.Parameters.AddWithValue("@email", Email);
                var count = (long)cm.ExecuteScalar();
                if (count == 0)
                    isTrue = false;
                else
                    isTrue = true;
            }

            return isTrue;
        }
    }
}
