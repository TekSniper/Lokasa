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
        private bool _isTrue { get; set; } = false;

        public bool Create()
        {
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
                cm.Parameters.AddWithValue("vpwd", BCrypt.Net.BCrypt.HashPassword(this.MotDePasse));
                cm.Parameters.AddWithValue("vfonction", this.Fonction);
                cm.Parameters.AddWithValue("vetat", this.Etat);
                var i = cm.ExecuteNonQuery();
                if (i != 0)
                    _isTrue = true;
                else
                    _isTrue = false;
            }

            return _isTrue;
        }
        public bool UpdatePwd()
        {
            using(var cnx = new DbConnexion().GetConnection())
            {
                cnx.Open();
                var cm = new MySqlCommand("update agent set mot_de_passe=@pwd where email=@email", cnx);
                cm.Parameters.AddWithValue("@pwd", BCrypt.Net.BCrypt.HashPassword(MotDePasse));
                cm.Parameters.AddWithValue("@email", this.Email);
                var i = cm.ExecuteNonQuery();
                if (i != 0)
                    _isTrue = true;
                else
                    _isTrue = false;
            }
            return _isTrue;
        }
        public bool ChangeStatus()
        {
            using(var cnx = new DbConnexion().GetConnection())
            {
                cnx.Open();
                var cm = new MySqlCommand("ChangerEtat", cnx);
                cm.CommandType= System.Data.CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("vetat",this.Etat);
                cm.Parameters.AddWithValue("vlogin",this.Email);
                var i = cm.ExecuteNonQuery();
                if (i != 0)
                    _isTrue = true;
                else
                    _isTrue = false;
            }

            return _isTrue;
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
                var cm = new MySqlCommand("select fonction from agent where email=@email", cnx);
                //var IdAgent = GetId();
                cm.Parameters.AddWithValue("@email", this.Email);
                var reader = cm.ExecuteReader();
                if (reader.Read())
                    fonction = reader.GetString(0);
            }

            return fonction;
        }
        public bool Authentification()
        {
            using (var cnx = new DbConnexion().GetConnection())
            {
                cnx.Open();
                var hachedPwd = string.Empty;
                var cm = new MySqlCommand("select mot_de_passe from agent where email=@email", cnx);
                cm.Parameters.AddWithValue("@email", this.Email);
                var reader = cm.ExecuteReader();
                if (reader.Read())
                    hachedPwd = reader.GetString(0);
                
                if (BCrypt.Net.BCrypt.Verify(MotDePasse, hachedPwd))
                    _isTrue = true;
                else
                    _isTrue = false;
            }

            return _isTrue;
        }
        public bool Exists()
        {
            using(var cnx = new DbConnexion().GetConnection())
            {
                cnx.Open();
                var cm = new MySqlCommand("select count(*) from agent where email=@email", cnx);
                cm.Parameters.AddWithValue("@email", Email);
                var count = (long)cm.ExecuteScalar();
                if (count == 0)
                    _isTrue = false;
                else
                    _isTrue = true;
            }

            return _isTrue;
        }
        public bool IsDefaultPassword(string ParamPwd)
        {
            using(var cnx = new DbConnexion().GetConnection())
            {
                cnx.Open();
                var hachedPwd = string.Empty;
                var cm = new MySqlCommand("select mot_de_passe from agent where email=@email", cnx);
                cm.Parameters.AddWithValue("@email", this.Email);
                var reader = cm.ExecuteReader();
                if(reader.Read())
                    hachedPwd = reader.GetString(0);
                
                if(BCrypt.Net.BCrypt.Verify(ParamPwd, hachedPwd))
                    _isTrue = true;
                else
                    _isTrue = false;
            }
            return _isTrue;
        }

        public string GetFullName()
        {
            var fullName = string.Empty;
            using (var cnx = new DbConnexion().GetConnection())
            {
                cnx.Open();
                using (var cm = new MySqlCommand("select nom,postnom,prenom from agent where id=@id", cnx))
                {
                    cm.Parameters.AddWithValue("@id", this.Id);
                    using (var reader = cm.ExecuteReader())
                    {
                        if(reader.Read())
                            fullName = reader.GetString(0)+" "+reader.GetString(1)+" "+reader.GetString(2);
                    }
                }
            }
            
            return fullName;
        }
    }
}
