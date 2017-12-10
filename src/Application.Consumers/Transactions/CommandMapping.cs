namespace PetProjects.Mts.CommandHandler.Application.Consumers.Transactions
{
    using CommandHandlers.Transaction;
    using Contract = PetProjects.MicroTransactions.Commands.Transactions.V1;

    public static class CommandMapping
    {
        public static CreateTransactionCommand ToModelCommand(this Contract.CreateTransactionCommand source)
        {
            return new CreateTransactionCommand
            {
                UserId = source.UserId,
                Quantity = source.Quantity,
                ItemId = source.ItemId,
                Timestamp = source.Timestamp.UnixTimeEpochTimestamp,
                TransactionId = source.TransactionId
            };
        }
    }
}