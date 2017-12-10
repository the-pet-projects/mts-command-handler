namespace PetProjects.Mts.CommandHandler.Infrastructure.Configurations.DependencyInjection
{
    using System;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Logging;

    using PetProjects.Framework.Consul.Store;
    using PetProjects.Framework.Logging.Producer;

    using Serilog.Events;

    public static class LoggingConfigurations
    {
        public static IServiceCollection LoadLoggingConfiguration(this IServiceCollection serviceCollection, IStringKeyValueStore configStore)
        {
            var kafkaConfig = new KafkaConfiguration
            {
                Brokers = configStore.GetAndConvertValue<string>("logging/kafka/brokersList").Split(','),
                Topic = configStore.GetAndConvertValue<string>("logging/kafka/topic")
            };

            var sinkConfig = new PeriodicSinkConfiguration
            {
                BatchSizeLimit = configStore.GetAndConvertValue<int>("logging/batchSizeLimit"),
                Period = TimeSpan.FromMilliseconds(configStore.GetAndConvertValue<int>("logging/periodMs"))
            };

            var logLevel = configStore.GetAndConvertValue<LogEventLevel>("logging/logLevel");
            var logType = configStore.GetAndConvertValue<string>("logging/logType");

            serviceCollection.AddLogging(builder => builder.AddPetProjectLogging(logLevel, sinkConfig, kafkaConfig, logType, true).AddConsole());
            serviceCollection.TryAddSingleton<ILogger>(sp => sp.GetRequiredService<ILoggerFactory>().CreateLogger("No category"));

            return serviceCollection;
        }
    }
}