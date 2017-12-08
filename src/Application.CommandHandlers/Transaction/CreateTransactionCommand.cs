namespace PetProjects.Mts.CommandHandler.Application.CommandHandlers.Transaction
{
    using System;
    using Framework.Cqrs.Commands;
    using Infrasctructure.CrossCutting.Error;

    public class CreateTransactionCommand : ICommand<CommandResult>
    {
        public CreateTransactionCommand()
        {
        }

        public Guid UserId { get; set; }

        public Guid ItemId { get; set; }

        public float Quantity { get; set; }

        public Guid TransactionId { get; set; }

        public long Timestamp { get; set; }
    }
}