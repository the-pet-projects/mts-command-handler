namespace PetProjects.Mts.CommandHandler.Presentation.ConsoleApplication.Consumer
{
    using System.Threading.Tasks;
    using Confluent.Kafka;
    using Framework.Kafka.Configurations.Consumer;
    using Framework.Kafka.Contracts.Topics;
    using Microsoft.Extensions.Logging;
    using PetProjects.Framework.Kafka.Consumer;
    using PetProjects.MicroTransactions.Commands.Transactions.V1;

    public class TransactionsConsumer : Consumer<TransactionCommand>
    {
        private readonly ILogger logger;
        private Task background;

        protected TransactionsConsumer(ILogger logger, ITopic<TransactionCommand> topic, IConsumerConfiguration configuration)
            : base(topic, configuration)
        {
            this.logger = logger;
            this.ConsumerHandlerFor<CreateTransaction>(this.HandleCreateCommand);

            // TODO: Need to check this later! Not tested :S 
            this.background = Task.Factory.StartNew(
                () =>
                {
                    var initiated = this.StartConsuming();

                    if (initiated)
                    {
                        this.logger.LogInformation("{0} was initiated.", nameof(TransactionsConsumer));
                    }
                    else
                    {
                        this.logger.LogCritical("{0} failed to start. Configurations: {config}", nameof(TransactionsConsumer), configuration);
                    }
                },
                TaskCreationOptions.LongRunning);
        }

        protected override void HandleStatistics(object sender, string statistics)
        {
        }

        protected override void HandleLogs(object sender, LogMessage logMessage)
        {
        }

        protected override void HandleError(object sender, Error error)
        {
        }

        protected override void HandleOnConsumerError(object sender, Message message)
        {
        }

        private void HandleCreateCommand(CreateTransaction cmd)
        {
        }
    }
}