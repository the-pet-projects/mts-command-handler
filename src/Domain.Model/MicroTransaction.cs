namespace PetProjects.Mts.CommandHandler.Domain.Model
{
    using System;

    public class MicroTransaction
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid ItemId { get; set; }

        public double Quantity { get; set; }

        public long Timestamp { get; set; }
    }
}