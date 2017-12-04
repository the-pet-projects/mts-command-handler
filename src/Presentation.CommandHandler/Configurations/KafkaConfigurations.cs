namespace PetProjects.Mts.CommandHandler.Presentation.ConsoleApplication.Configurations
{
    using System;
    using Consumer;
    using Framework.Consul.Store;
    using Framework.Kafka.Configurations.Consumer;
    using Framework.Kafka.Configurations.Producer;
    using Framework.Kafka.Consumer;
    using Framework.Kafka.Contracts.Topics;
    using Framework.Kafka.Producer;
    using Microsoft.Extensions.DependencyInjection;
    using MicroTransactions.Commands.Transactions.V1;
    using MicroTransactions.Events.Transactions.V1;

    public static class KafkaConfigurations
    {
        public static IServiceCollection LoadConsumersConfigurations(this IServiceCollection serviceCollection, IStringKeyValueStore configStore)
        {
            serviceCollection.AddSingleton<IConsumerConfiguration>(sp =>
            {
                var pollTimeout = configStore.GetAndConvertValue<int>("kafka/consumer/pollTimeout");
                var brokers = configStore.GetAndConvertValue<string>("kafka/brokers").Split(',');
                var groupId = configStore.GetAndConvertValue<string>("kafka/consumer/groupId");
                var clientIdPrefix = configStore.GetAndConvertValue<string>("kafka/consumer/clientIdPrefix");

                return new ConsumerConfiguration(
                        groupId,
                        $"{clientIdPrefix}-{Guid.NewGuid()}",
                        brokers)
                    .SetPollTimeout(pollTimeout);
            });
            
            serviceCollection.AddSingleton<ITopic<TransactionCommand>, TransactionCommandsTopic>();

            serviceCollection.AddSingleton<IConsumer<TransactionCommand>, TransactionsConsumer>();

            return serviceCollection;
        }

        public static IServiceCollection LoadProducersConfigurations(this IServiceCollection serviceCollection, IStringKeyValueStore configStore)
        {
            var brokers = configStore.GetAndConvertValue<string>("kafka/brokers").Split(',');
            var clientId = configStore.GetAndConvertValue<string>("kafka/producer/clientId");

            var producerConfiguration = new ProducerConfiguration(clientId, brokers);

            serviceCollection.AddSingleton<IProducer<TransactionEvent>>(
                new Producer<TransactionEvent>(new TransactionEventsTopic(), producerConfiguration));

            return serviceCollection;
        }
    }
}