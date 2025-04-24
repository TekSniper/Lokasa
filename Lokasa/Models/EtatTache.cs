using System.ComponentModel.DataAnnotations;

namespace Lokasa.Models;

public class EtatTache
{
    [Key]
    public byte Id { get; set; }
    [Required]
    public string Designation { get; set; }

    public string GetDesignation()
    {
        using (var cnx = new DbConnexion().GetConnection())
        {
            cnx.Open();
            using (var cm = new MySqlCommand("select designation from etat where id = @Id",cnx))
            {
                cm.Parameters.AddWithValue("@Id", Id);
                using (var reader = cm.ExecuteReader())
                {
                    if(reader.Read())
                        this.Designation = reader.GetString("designation");
                }
            }

            return Designation;
        }
    }
}