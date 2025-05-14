namespace Lokasa.Repositories
{
    public static class AppConnection
    {
        static IConfigurationRoot root()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            return builder.Build();
        }
        static string connectionString => root().GetConnectionString("lokasa_sql")!;
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
