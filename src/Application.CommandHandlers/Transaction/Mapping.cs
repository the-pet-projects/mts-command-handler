namespace PetProjects.Mts.CommandHandler.Application.CommandHandlers.Transaction
{
    using Domain.Model;
    using Framework.Kafka.Contracts.Utils;
    using MicroTransactions.Events.Transactions.V1;

    public static class Mapping
    {
        public static MicroTransaction ToModel(this CreateTransactionCommand source)
        {
            return new MicroTransaction
            {
                UserId = source.UserId,
                Quantity = source.Quantity,
                ItemId = source.ItemId,
                Timestamp = source.Timestamp,
                Id = source.TransactionId
            };
        }

        public static TransactionCreatedEvent ToEvent(this MicroTransaction source)
        {
            return new TransactionCreatedEvent
            {
                TransactionId = source.Id,
                UserId = source.UserId,
                ItemId = source.ItemId,
                Quantity = source.Quantity,
                Timestamp = new Timestamp()
            };
        }
    }
}