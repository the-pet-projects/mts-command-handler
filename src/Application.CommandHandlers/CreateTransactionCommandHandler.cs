namespace PetProjects.Mts.CommandHandler.Application.CommandHandlers
{
    using System.Threading.Tasks;

    using PetProjects.Framework.Cqrs.Commands;

    using PetProjects.Mts.CommandHandler.Application.CommandHandlers.Transaction;
    using PetProjects.Mts.CommandHandler.Data.Repository.CassandraDb.Transactions;
    using PetProjects.Mts.CommandHandler.Domain.Model;
    using PetProjects.Mts.CommandHandler.Infrasctructure.CrossCutting.Error;

    public class CreateTransactionCommandHandlerAsync : ICommandHandlerWithResponseAsync<CreateTransactionCommand, CommandResult>
    {
        private readonly ITransactionsRepository transactionsRepository;

        public CreateTransactionCommandHandlerAsync(ITransactionsRepository transactionsRepository)
        {
            this.transactionsRepository = transactionsRepository;
        }

        public async Task<CommandResult> HandleAsync(CreateTransactionCommand command)
        {
            return await this.transactionsRepository.InsertAsync(new MicroTransaction());
        }
    }
}