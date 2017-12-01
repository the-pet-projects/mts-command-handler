namespace PetProjects.Mts.CommandHandler.Presentation.ConsoleApplication.Configurations
{
    using System.IO;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Bootstrapper
    {
        public Bootstrapper()
        {
            this.SetupConfiguration();
        }

        public IConfigurationRoot Configuration { get; private set; }

        public IServiceCollection ServiceCollection { get; private set; }

        private void SetupConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables("MTS_APP_SETTINGS");

            this.Configuration = configurationBuilder.Build();
        }

        public Bootstrapper BootstrapContainer()
        {
            this.ServiceCollection = new ServiceCollection();
            
            // Setup Services, repositories, Producers, consumers, etc

            return this;
        }
    }
}