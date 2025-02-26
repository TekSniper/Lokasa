using MySql.Data.MySqlClient;

namespace Lokasa.Models
{
    public class Service
    {
        public short Id { get; set; }
        public string Designation { get; set; } = string.Empty;

        public bool CreateService()
        {
            var isTrue = false;
            using(var cnx = new DbConnexion().GetConnection())
            {
                cnx.Open();
                var cm = new MySqlCommand("CreerService",cnx);
                cm.CommandType = System.Data.CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("vdesignation",this.Designation);
                var i = cm.ExecuteNonQuery();
                if (i != 0)
                    isTrue = true;
                else
                    isTrue = false;
            }

            return isTrue;
        }
    }
}
