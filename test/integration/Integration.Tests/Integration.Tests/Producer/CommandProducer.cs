namespace Integration.Tests.Producer
{
    using System;
    using Configs;
    using PetProjects.Framework.Kafka.Configurations.Producer;
    using PetProjects.Framework.Kafka.Producer;
    using PetProjects.MicroTransactions.Commands.Transactions.V1;

    internal static class CommandProducer
    {
        private static readonly Lazy<IProducer<TransactionCommandV1>> LazyProducer = BuildProducer();

        public static IProducer<TransactionCommandV1> Producer => LazyProducer.Value;

        private static Lazy<IProducer<TransactionCommandV1>> BuildProducer()
        {
            return new Lazy<IProducer<TransactionCommandV1>>(() => new Producer<TransactionCommandV1>(
                new TransactionCommandsTopicV1(AppSettings.Current.KafkaTopicEnvironment), 
                new ProducerConfiguration("mts-command-handler-integrationtests", AppSettings.Current.KafkaBrokers)));
        }
    }
}