namespace PetProjects.Mts.CommandHandler.Presentation.ConsoleApplication.Configurations
{
    using System.IO;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Logging;
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
            var configStore = GetConfigurationKeyValueStore(this.Configuration);

            this.ServiceCollection = new ServiceCollection();

            this.ServiceCollection.AddPetProjectConsulServices(this.Configuration, true);
            this.ServiceCollection.LoadLoggingConfiguration(configStore);
            this.ServiceCollection.LoadCassandraConfigurations(configStore);
            this.ServiceCollection.LoadRepositoriesConfigurations();
            this.ServiceCollection.LoadCommandHandlersConfigurations();
            this.ServiceCollection.LoadConsumersConfigurations(configStore);
            this.ServiceCollection.LoadProducersConfigurations(configStore);

            return this;
        }

        private static IStringKeyValueStore GetConfigurationKeyValueStore(IConfiguration configuration)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddPetProjectConsulServices(configuration, true);
            serviceCollection.AddLogging(builder => builder.AddConsole());
            serviceCollection.TryAddSingleton<ILogger>(sp => sp.GetRequiredService<ILoggerFactory>().CreateLogger("No category"));

            using (var tempProvider = serviceCollection.BuildServiceProvider())
            {
                using (var scopedProvider = tempProvider.CreateScope())
                {
                    return scopedProvider.ServiceProvider.GetRequiredService<IStringKeyValueStore>();
                }
            }
        }

        private void SetupConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Configurations/appsettings.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables("MTS_APP_SETTINGS_");

            this.Configuration = configurationBuilder.Build();
        }
    }
}