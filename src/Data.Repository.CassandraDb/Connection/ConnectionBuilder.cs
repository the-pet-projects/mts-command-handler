namespace PetProjects.Mts.CommandHandler.Data.Repository.CassandraDb.Connection
{
    using System.Transactions;
    using Cassandra;
    using Cassandra.Data.Linq;
    using Cassandra.Mapping;
    using Configuration;
    using Domain.Model;
    using Transactions;

    public static class ConnectionBuilder
    {
        public static IConnection BuildConnection(CassandraConfiguration config)
        {
            var cluster = Cluster.Builder()
                .WithDefaultKeyspace(config.Keyspace)
                .AddContactPoints(config.ContactPoints)
                .Build();

            var mapConfig = new MappingConfiguration();
            mapConfig.Define<MicroTransactionMappings>();

            var session = cluster.ConnectAndCreateDefaultKeyspaceIfNotExists(config.ReplicationParameters);

            var table = new Table<MicroTransaction>(session, mapConfig);
            table.CreateIfNotExists();

            return new Connection(cluster, session, mapConfig);
        }
    }
}