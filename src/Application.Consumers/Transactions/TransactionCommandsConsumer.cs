namespace PetProjects.Mts.CommandHandler.Application.Consumers.Transactions
{
    using System.Threading.Tasks;
    using CommandHandlers.Transaction;
    using Framework.Cqrs.Mediator;
    using Framework.Kafka.Configurations.Consumer;
    using Framework.Kafka.Consumer;
    using Framework.Kafka.Contracts.Topics;
    using Infrasctructure.CrossCutting.Error;
    using Microsoft.Extensions.Logging;
    using MicroTransactions.Commands.Transactions.V1;

    public class TransactionsConsumer : Consumer<TransactionCommand>
    {
        private readonly ISimpleMediator mediator;
        private readonly ILogger logger;

        protected TransactionsConsumer(ISimpleMediator mediator, ILogger logger, ITopic<TransactionCommand> topic, IConsumerConfiguration configuration)
            : base(topic, configuration, logger)
        {
            this.mediator = mediator;
            this.logger = logger;
            this.Receive<CreateTransaction>(async message => await this.HandleCreateCommand(message));

            this.StartConsuming();
        }

        private async Task HandleCreateCommand(CreateTransaction cmd)
        {
            var result = await this.mediator.RunCommandAsync<CreateTransactionCommand, CommandResult>(new CreateTransactionCommand());

            if (result.Success)
            {
               await this.CommitAsync();
            }
            else
            {
                this.logger.LogError("Error Handling Create Command. Message was not committed. Result {result}", result);
            }
        }
    }
}