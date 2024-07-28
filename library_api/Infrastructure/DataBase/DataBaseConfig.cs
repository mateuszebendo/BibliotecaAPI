namespace library_api.Infrastructure.DataBase;

public static class DatabaseConfig
{
    public static string ConnectionString { get; private set; }
    
    public static void Initialize(IConfiguration configuration)
    {
        ConnectionString = configuration.GetConnectionString("SqlConnection");
    }
}
