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
                var clientIdPrefix = configStore.GetAndConvertValue<string>("kafka/consumer/clientId");

                return new ConsumerConfiguration(
                        groupId,
                        $"{clientIdPrefix}-{Guid.NewGuid()}",
                        brokers)
                    .SetPollIntervalInMs(pollTimeout);
            });

            var environment = configStore.GetAndConvertValue<string>("kafka/environment");

            serviceCollection.AddSingleton<ITopic<TransactionCommandV1>>(new TransactionCommandsTopicV1(environment));
            serviceCollection.AddTransient<IConsumer<TransactionCommandV1>, TransactionsCommandsConsumer>();

            return serviceCollection;
        }

        public static IServiceCollection LoadProducersConfigurations(this IServiceCollection serviceCollection, IStringKeyValueStore configStore)
        {
            var brokers = configStore.GetAndConvertValue<string>("kafka/brokersList").Split(',');
            var clientId = configStore.GetAndConvertValue<string>("kafka/producer/clientId");
            var environment = configStore.GetAndConvertValue<string>("kafka/environment");

            var producerConfiguration = new ProducerConfiguration(clientId, brokers);

            serviceCollection.AddSingleton<IProducer<TransactionEventV1>>(new Producer<TransactionEventV1>(new TransactionEventsTopicV1(environment), producerConfiguration));

            return serviceCollection;
        }
    }
}