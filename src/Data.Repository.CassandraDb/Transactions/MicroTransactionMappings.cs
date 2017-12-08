namespace PetProjects.Mts.CommandHandler.Data.Repository.CassandraDb.Transactions
{
    using Cassandra.Mapping;
    using Domain.Model;

    public class MicroTransactionMappings : Mappings
    {
        public MicroTransactionMappings()
        {
            this.For<MicroTransaction>()
                .TableName("transactions")
                .PartitionKey(t => t.UserId)
                .Column(t => t.Id, cfg => cfg.WithName("transaction_id"))
                .Column(t => t.UserId, cfg => cfg.WithName("user_id"))
                .Column(t => t.Timestamp, cfg => cfg.WithName("timestamp"))
                .Column(t => t.ItemId, cfg => cfg.WithName("item_id"))
                .Column(t => t.Quantity, cfg => cfg.WithName("quantity"))
                .ExplicitColumns();
        }
    }
}