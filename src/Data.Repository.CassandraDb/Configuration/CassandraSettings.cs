namespace PetProjects.Mts.CommandHandler.Data.Repository.CassandraDb.Configuration
{
    using Cassandra;

    public class CassandraSettings
    {
        public ConsistencyLevel TransactionsWriteConsistencyLevel { get; set; }
    }
}