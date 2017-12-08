namespace PetProjects.Mts.CommandHandler.Application.CommandHandlers.Transaction
{
    using System;

    using PetProjects.Framework.Cqrs.Commands;
    using PetProjects.Mts.CommandHandler.Domain.Model;
    using PetProjects.Mts.CommandHandler.Infrasctructure.CrossCutting.Error;

    public class CreateTransactionCommand : ICommand<CommandResult<MicroTransaction>>
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