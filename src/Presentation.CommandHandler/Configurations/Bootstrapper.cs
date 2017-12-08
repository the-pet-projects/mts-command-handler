namespace PetProjects.Mts.CommandHandler.Presentation.ConsoleApplication.Configurations
{
    using System.IO;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;

    using PetProjects.Framework.Consul;
    using PetProjects.Framework.Consul.Store;
    using PetProjects.Mts.CommandHandler.Infrastructure.Configurations.DependencyInjection;

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

            this.ServiceCollection.AddPetProjectConsulServices(this.Configuration, true);
            this.ServiceCollection.AddSingleton<ILogger>(NullLogger.Instance);

            IStringKeyValueStore configStore;

            using (var tmpServiceProvider = this.ServiceCollection.BuildServiceProvider())
            {
                configStore = tmpServiceProvider.GetRequiredService<IStringKeyValueStore>();
            }

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