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
        private readonly IProducer<TransactionEvent> producer;
        private readonly ILogger logger;

        public CreateTransactionCommandHandlerAsync(ITransactionsRepository transactionsRepository, IProducer<TransactionEvent> producer, ILogger logger)
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

            this.logger.LogTrace("Before Producing {event}: {values}", nameof(TransactionCreated), result.Data);

            var report = await this.producer.ProduceAsync(
                new TransactionCreated(
                    result.Data.Id,
                    result.Data.ItemId,
                    result.Data.Quantity,
                    result.Data.UserId,
                    new Timestamp()));

            if (!report.Error.HasError)
            {
                this.logger.LogWarning("Error Producing {event}. Error: {error}", nameof(TransactionCreated), report.Error);
            }

            this.logger.LogTrace("Produced {event} with {report}", nameof(TransactionCreated), report);


            return result;
        }
    }
}