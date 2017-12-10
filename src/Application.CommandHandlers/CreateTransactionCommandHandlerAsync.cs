namespace PetProjects.Mts.CommandHandler.Application.CommandHandlers
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using MicroTransactions.Events.Transactions.V1;

    using PetProjects.Framework.Cqrs.Commands;
    using PetProjects.Framework.Kafka.Contracts.Utils;
    using PetProjects.Framework.Kafka.Producer;
    using PetProjects.Mts.CommandHandler.Application.CommandHandlers.Transaction;
    using PetProjects.Mts.CommandHandler.Data.Repository.CassandraDb.Transactions;
    using PetProjects.Mts.CommandHandler.Domain.Model;
    using PetProjects.Mts.CommandHandler.Infrasctructure.CrossCutting.Error;

    public class CreateTransactionCommandHandlerAsync : ICommandHandlerWithResponseAsync<CreateTransactionCommand, CommandResult<MicroTransaction>>
    {
        private readonly ITransactionsRepository transactionsRepository;
        private readonly IProducer<TransactionEventV1> producer;
        private readonly ILogger logger;

        public CreateTransactionCommandHandlerAsync(ITransactionsRepository transactionsRepository, IProducer<TransactionEventV1> producer, ILogger logger)
        {
            this.transactionsRepository = transactionsRepository;
            this.producer = producer;
            this.logger = logger;
        }

        public async Task<CommandResult<MicroTransaction>> HandleAsync(CreateTransactionCommand command)
        {
            this.logger.LogTrace("Entered {commandHandler} with command values: {command}", nameof(CreateTransactionCommandHandlerAsync), command);
            var result = await this.transactionsRepository.InsertAsync(new MicroTransaction());

            this.logger.LogTrace("After Insert Action {commandHandler} result was: {result}", nameof(CreateTransactionCommandHandlerAsync), result);
            if (!result.Success)
            {
                return result;
            }

            this.logger.LogTrace("Before Producing {event}: {values}", nameof(TransactionCreatedEvent), result.Data);

            var report = await this.producer.ProduceAsync(
                new TransactionCreatedEvent
                {
                    TransactionId = result.Data.Id,
                    UserId = result.Data.UserId,
                    ItemId = result.Data.ItemId,
                    Quantity = result.Data.Quantity,
                    Timestamp = new Timestamp()
                });

            if (!report.Error.HasError)
            {
                this.logger.LogWarning("Error Producing {event}. Error: {error}", nameof(TransactionCreatedEvent), report.Error);
            }

            this.logger.LogTrace("Produced {event} with {report}", nameof(TransactionCreatedEvent), report);


            return result;
        }
    }
}