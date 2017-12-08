namespace PetProjects.Mts.CommandHandler.Data.Repository.CassandraDb.Transactions
{
    using System.Threading.Tasks;

    using PetProjects.Mts.CommandHandler.Domain.Model;
    using PetProjects.Mts.CommandHandler.Infrasctructure.CrossCutting.Error;

    public interface ITransactionsRepository
    {
        Task<CommandResult<MicroTransaction>> InsertAsync(MicroTransaction transaction);
    }
}