namespace PetProjects.Mts.CommandHandler.Infrastructure.Configurations.DependencyInjection
{
    using System;

    using Microsoft.Extensions.DependencyInjection;

    using PetProjects.Framework.Consul.Store;
    using PetProjects.Framework.Kafka.Configurations.Consumer;
    using PetProjects.Framework.Kafka.Configurations.Producer;
    using PetProjects.Framework.Kafka.Consumer;
    using PetProjects.Framework.Kafka.Contracts.Topics;
    using PetProjects.Framework.Kafka.Producer;
    using PetProjects.MicroTransactions.Commands.Transactions.V1;
    using PetProjects.MicroTransactions.Events.Transactions.V1;
    using PetProjects.Mts.CommandHandler.Application.Consumers.Transactions;

    public static class KafkaConfigurations
    {
        public static IServiceCollection LoadConsumersConfigurations(this IServiceCollection serviceCollection, IStringKeyValueStore configStore)
        {
            serviceCollection.AddSingleton<IConsumerConfiguration>(sp =>
            {
                var pollTimeout = configStore.GetAndConvertValue<int>("kafka/consumer/pollTimeout");
                var brokers = configStore.GetAndConvertValue<string>("kafka/brokersList").Split(',');
                var groupId = configStore.GetAndConvertValue<string>("kafka/consumer/consumerGroupId");
                var clientIdPrefix = configStore.GetAndConvertValue<string>("kafka/consumer/clientIdPrefix");

                return new ConsumerConfiguration(
                        groupId,
                        $"{clientIdPrefix}-{Guid.NewGuid()}",
                        brokers)
                    .SetPollIntervalInMs(pollTimeout);
            });

            serviceCollection.AddSingleton<ITopic<TransactionCommand>, TransactionCommandsTopic>();

            serviceCollection.AddTransient<IConsumer<TransactionCommand>, TransactionsConsumer>();

            return serviceCollection;
        }

        public static IServiceCollection LoadProducersConfigurations(this IServiceCollection serviceCollection, IStringKeyValueStore configStore)
        {
            var brokers = configStore.GetAndConvertValue<string>("kafka/brokers").Split(',');
            var clientId = configStore.GetAndConvertValue<string>("kafka/producer/clientId");

            var producerConfiguration = new ProducerConfiguration(clientId, brokers);

            serviceCollection.AddSingleton<IProducer<TransactionEvent>>(new Producer<TransactionEvent>(new TransactionEventsTopic(), producerConfiguration));

            return serviceCollection;
        }
    }
}