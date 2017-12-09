namespace PetProjects.Mts.CommandHandler.Infrastructure.Configurations.DependencyInjection
{
    using System;
    using System.Collections.Generic;

    using Cassandra;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;

    using PetProjects.Framework.Consul.Store;
    using PetProjects.Mts.CommandHandler.Data.Repository.CassandraDb.Configuration;

    public static class CassandraConfigurations
    {
        public static IServiceCollection LoadCassandraConfigurations(this IServiceCollection serviceCollection,
            IStringKeyValueStore configStore)
        {
            serviceCollection.AddTransient<CassandraSettings>(serviceProvider => new CassandraSettings
            {
                TransactionsWriteConsistencyLevel = (ConsistencyLevel)Enum.Parse(typeof(ConsistencyLevel), configStore.GetAndConvertValue<string>("cassandra/consistencyLevel/write"))
            });

            serviceCollection.AddSingleton<CassandraConfiguration>(serviceProvider => new CassandraConfiguration
            {
                ContactPoints = configStore.GetAndConvertValue<string>("cassandra/contactPoints").Split(','),
                Keyspace = configStore.GetAndConvertValue<string>("cassandra/keyspace"),
                ReplicationParameters = JsonConvert.DeserializeObject<Dictionary<string, string>>(configStore.GetAndConvertValue<string>("cassandra/replicationParameters"))
            });

            return serviceCollection;
        }
    }
}