namespace PetProjects.Mts.CommandHandler.Application.CommandHandlers
{
    using System.Threading.Tasks;

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

        public CreateTransactionCommandHandlerAsync(ITransactionsRepository transactionsRepository, IProducer<TransactionEvent> producer)
        {
            this.transactionsRepository = transactionsRepository;
            this.producer = producer;
        }

        public async Task<CommandResult<MicroTransaction>> HandleAsync(CreateTransactionCommand command)
        {
            var result = await this.transactionsRepository.InsertAsync(new MicroTransaction());

            if (!result.Success)
            {
                return result;
            }

            this.producer.Produce(
                new TransactionCreated(
                    result.Data.Id,
                    result.Data.ItemId,
                    result.Data.Quantity,
                    result.Data.UserId,
                    new Timestamp()));

            return result;
        }
    }
}