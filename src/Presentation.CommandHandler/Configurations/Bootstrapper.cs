namespace PetProjects.Mts.CommandHandler.Presentation.ConsoleApplication.Configurations
{
    using System.IO;
    using Framework.Consul;
    using Framework.Consul.Store;
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

        public Bootstrapper BootstrapContainer()
        {
            this.ServiceCollection = new ServiceCollection();

            var sp = this.ServiceCollection.BuildServiceProvider();

            this.ServiceCollection.AddPetProjectConsulServices(this.Configuration, true);

            var configStore = sp.GetRequiredService<IStringKeyValueStore>();

            this.ServiceCollection
                .LoadConsumersConfigurations(configStore)
                .LoadProducersConfigurations(configStore)
                .LoadLoggingConfiguration(configStore);

            return this;
        }

        private void SetupConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables("MTS_APP_SETTINGS");

            this.Configuration = configurationBuilder.Build();
        }
    }
}