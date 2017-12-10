namespace PetProjects.Mts.CommandHandler.Application.CommandHandlers
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using MicroTransactions.Events.Transactions.V1;

    using PetProjects.Framework.Cqrs.Commands;
    using PetProjects.Framework.Kafka.Producer;
    using PetProjects.Mts.CommandHandler.Application.CommandHandlers.Transaction;
    using PetProjects.Mts.CommandHandler.Data.Repository.CassandraDb.Transactions;
    using PetProjects.Mts.CommandHandler.Domain.Model;
    using PetProjects.Mts.CommandHandler.Infrasctructure.CrossCutting.Error;

    public class CreateTransactionCommandHandlerAsync : ICommandHandlerWithResponseAsync<CreateTransactionCommand, CommandResult<MicroTransaction>>
    {
        private readonly ITransactionsRepository transactionsRepository;
        private readonly IProducer<TransactionEventV1> producer;
        private readonly ILogger<CreateTransactionCommandHandlerAsync> logger;

        public CreateTransactionCommandHandlerAsync(ITransactionsRepository transactionsRepository, IProducer<TransactionEventV1> producer, ILogger<CreateTransactionCommandHandlerAsync> logger)
        {
            this.transactionsRepository = transactionsRepository;
            this.producer = producer;
            this.logger = logger;
        }

        public async Task<CommandResult<MicroTransaction>> HandleAsync(CreateTransactionCommand command)
        {
            this.logger.LogTrace("Entered {commandHandler}", nameof(CreateTransactionCommandHandlerAsync));

            var result = await this.transactionsRepository.InsertAsync(command.ToModel());

            if (!result.Success)
            {
                this.logger.LogTrace("Left {commandHandler} with Success: {result}", nameof(CreateTransactionCommandHandlerAsync), result.Success);
                return result;
            }

            this.logger.LogTrace("Before Producing {eventType}", nameof(TransactionCreatedEvent));

            var report = await this.producer.ProduceAsync(result.Data.ToEvent());

            this.logger.LogTrace("After Producing {eventType}", nameof(TransactionCreatedEvent));

            if (report.Error.HasError)
            {
                this.logger.LogWarning("Error Producing {event}. Error: {error}", nameof(TransactionCreatedEvent), new { Error = report.Error });
            }

            this.logger.LogTrace("Left {commandHandler} with Success: {result}", nameof(CreateTransactionCommandHandlerAsync), result.Success);
            return result;
        }
    }
}