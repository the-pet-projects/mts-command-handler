namespace Integration.Tests.Transactions
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Cassandra;
    using Configs;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PetProjects.MicroTransactions.Commands.Transactions.V1;
    using Producer;
    using Tests;

    [TestClass]
    public class TransactionCommandTests
    {
        [TestMethod]
        public async Task ConsumeTransactionCreated_ValidCommand_TransactionWasInsertedInDb()
        {
            // Arrange
            var transaction = new CreateTransactionCommand
            {
                TransactionId = Utils.GenerateTransactionId(),
                ItemId = Guid.NewGuid(),
                Quantity = 1,
                UserId = Utils.GenerateUserId()
            };

            // Act
            await CommandProducer.Producer.ProduceAsync(transaction).ConfigureAwait(false);

            // Assert
            var result = await Utils.AssertWithRetry<Row>(async () =>
            {
                var st = new SimpleStatement($"SELECT * FROM {AppSettings.Current.CassandraKeyspace}.transactions WHERE user_id = ?", transaction.UserId);
                var rs = await CassandraConnection.Session.ExecuteAsync(st).ConfigureAwait(false);
                var row = rs.ToList().FirstOrDefault();

                row.Should().NotBeNull();
                return row;
            });

            result.GetValue<Guid>("transaction_id").Should().Be(transaction.TransactionId);
            result.GetValue<long>("timestamp").Should().Be(transaction.Timestamp.UnixTimeEpochTimestamp);
            result.GetValue<Guid>("user_id").Should().Be(transaction.UserId);
            result.GetValue<int>("quantity").Should().Be(transaction.Quantity);
            result.GetValue<Guid>("item_id").Should().Be(transaction.ItemId);
        }
    }
}