using MySql.Data.MySqlClient;
namespace Lokasa.Models
{
    public class DbConnexion
    {        
        private string _connectionString { get; set; } = string.Empty;
        IConfigurationRoot root()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            return builder.Build();
        }
        public DbConnexion()
        {
            //this._connectionString = root().GetSection("ConnectionStrings").GetSection("furs@").Value!;
            this._connectionString = root().GetSection("ConnectionStrings").GetSection("lok@$@").Value!;
        }
        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(this._connectionString);
        }
    }
}
