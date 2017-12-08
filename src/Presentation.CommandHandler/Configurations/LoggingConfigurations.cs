namespace PetProjects.Mts.CommandHandler.Presentation.ConsoleApplication.Configurations
{
    using System;
    using Framework.Consul.Store;
    using Framework.Logging.Producer;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog.Events;

    public static class LoggingConfigurations
    {
        public static IServiceCollection LoadLoggingConfiguration(this IServiceCollection serviceCollection, IStringKeyValueStore configStore)
        {
            var kafkaConfig = new KafkaConfiguration
            {
                Brokers = configStore.GetAndConvertValue<string>("kafka/brokers").Split(','),
                Topic = configStore.GetAndConvertValue<string>("kafka/logging/topic")
            };

            var sinkConfig = new PeriodicSinkConfiguration
            {
                BatchSizeLimit = configStore.GetAndConvertValue<int>("logging/batchSizeLimit"),
                Period = TimeSpan.FromMilliseconds(configStore.GetAndConvertValue<int>("logging/periodMs"))
            };

            var logLevel = configStore.GetAndConvertValue<LogEventLevel>("logging/logLevel");
            var logType = configStore.GetAndConvertValue<string>("logging/logType");

            serviceCollection.AddLogging(builder => builder.AddPetProjectLogging(logLevel, sinkConfig, kafkaConfig, logType, true));

            return serviceCollection;
        }
    }
}