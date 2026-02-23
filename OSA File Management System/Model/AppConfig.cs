using Microsoft.Extensions.Configuration;

namespace OSA_File_Management_System.Model
{
    public static class AppConfig
    {
        private static IConfiguration? _configuration;

        public static IConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    _configuration = new ConfigurationBuilder()
                        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .Build();
                }
                return _configuration;
            }
        }

        public static string GetConnectionString()
        {
            return Configuration.GetConnectionString("DefaultConnection") 
                ?? "SERVER=localhost;DATABASE=osasystem;UID=osa_network;PASSWORD=OsaSystem0727;";
        }
    }
}
