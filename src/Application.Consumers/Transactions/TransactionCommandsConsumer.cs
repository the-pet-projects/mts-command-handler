namespace PetProjects.Mts.CommandHandler.Application.Consumers.Transactions
{
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using PetProjects.Framework.Cqrs.Mediator;
    using PetProjects.Framework.Kafka.Configurations.Consumer;
    using PetProjects.Framework.Kafka.Consumer;
    using PetProjects.Framework.Kafka.Contracts.Topics;
    using PetProjects.Mts.CommandHandler.Application.CommandHandlers.Transaction;
    using PetProjects.Mts.CommandHandler.Domain.Model;
    using PetProjects.Mts.CommandHandler.Infrasctructure.CrossCutting.Error;

    using Contract = PetProjects.MicroTransactions.Commands.Transactions.V1;

    public class TransactionsCommandsConsumer: Consumer<Contract.TransactionCommandV1>
    {
        private readonly ISimpleMediator mediator;
        private readonly ILogger<TransactionsCommandsConsumer> logger;

        public TransactionsCommandsConsumer(ISimpleMediator mediator, ILogger<TransactionsCommandsConsumer> logger, ITopic<Contract.TransactionCommandV1> topic, IConsumerConfiguration configuration)
            : base(topic, configuration, logger)
        {
            this.mediator = mediator;
            this.logger = logger;
            this.TryReceiveAsync<Contract.CreateTransactionCommand>(async (command) => await this.HandleCreateCommandAsync(command));

            this.logger.LogInformation("Consumer {type} has started...", nameof(TransactionsCommandsConsumer));
        }

        private async Task<bool> HandleCreateCommandAsync(Contract.CreateTransactionCommand command)
        {
            var result = await this.mediator.RunCommandAsync<CreateTransactionCommand, CommandResult<MicroTransaction>>(new CreateTransactionCommand
            {
                UserId = command.UserId,
                Quantity = command.Quantity,
                ItemId = command.ItemId,
                Timestamp = command.Timestamp.UnixTimeEpochTimestamp,
                TransactionId = command.TransactionId
            });

            if (!result.Success)
            {
                this.logger.LogWarning("Command {command} was unsuccessful. Result: {result}", command, result);
            }

            return result.Success;
        }
    }
}