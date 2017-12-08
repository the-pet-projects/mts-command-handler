namespace PetProjects.Mts.CommandHandler.Data.Repository.CassandraDb.Transactions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Cassandra.Mapping;

    using PetProjects.Mts.CommandHandler.Data.Repository.CassandraDb.Configuration;
    using PetProjects.Mts.CommandHandler.Data.Repository.CassandraDb.Connection;
    using PetProjects.Mts.CommandHandler.Domain.Model;
    using PetProjects.Mts.CommandHandler.Infrasctructure.CrossCutting.Error;

    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly IConnection connection;
        private readonly CassandraSettings settings;

        public TransactionsRepository(IConnection connection, CassandraSettings settings)
        {
            this.connection = connection;
            this.settings = settings;
        }

        public Task<CommandResult<MicroTransaction>> InsertAsync(MicroTransaction transaction)
        {
            var result = new CommandResult<MicroTransaction>(transaction);

            try
            {
                this.connection.Mapper.InsertAsync(
                    transaction,
                    false,
                    null,
                    CqlQueryOptions.New().SetConsistencyLevel(this.settings.TransactionsWriteConsistencyLevel));

                return Task.FromResult(result);
            }
            catch (Exception exception)
            {
                result.Add(new Error
                {
                    ErrorCode = 1,
                    Message = "Error ocurred while performing insert in cassandra",
                    Exceptions = new List<Exception>
                    {
                        new Exception("Error ocurred while performing insert in cassandra", exception.InnerException)
                    }
                });

                return Task.FromResult(result);
            }
        }
    }
}