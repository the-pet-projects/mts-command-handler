namespace PetProjects.Mts.CommandHandler.Infrastructure.Configurations.DependencyInjection
{
    using Data.Repository.CassandraDb.Configuration;
    using Data.Repository.CassandraDb.Connection;
    using Data.Repository.CassandraDb.Transactions;
    using Microsoft.Extensions.DependencyInjection;

    public static class RepositoriesConfigurations
    {
        public static IServiceCollection LoadRepositoriesConfigurations(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(sp => ConnectionBuilder.BuildConnection(sp.GetRequiredService<CassandraConfiguration>()));

            serviceCollection.AddTransient<ITransactionsRepository, TransactionsRepository>();

            return serviceCollection;
        }
    }
}