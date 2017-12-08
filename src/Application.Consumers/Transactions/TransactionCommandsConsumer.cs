namespace PetProjects.Mts.CommandHandler.Application.Consumers.Transactions
{
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using PetProjects.Framework.Cqrs.Mediator;
    using PetProjects.Framework.Kafka.Configurations.Consumer;
    using PetProjects.Framework.Kafka.Consumer;
    using PetProjects.Framework.Kafka.Contracts.Topics;
    using PetProjects.MicroTransactions.Commands.Transactions.V1;
    using PetProjects.Mts.CommandHandler.Application.CommandHandlers.Transaction;
    using PetProjects.Mts.CommandHandler.Domain.Model;
    using PetProjects.Mts.CommandHandler.Infrasctructure.CrossCutting.Error;

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
            var result = await this.mediator.RunCommandAsync<CreateTransactionCommand, CommandResult<MicroTransaction>>(new CreateTransactionCommand());

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