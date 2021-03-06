﻿namespace Integration.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    internal static class Utils
    {
        private const int NumberOfAssertRetries = 40;
        private static readonly TimeSpan DelayBetweenAssertRetries = TimeSpan.FromMilliseconds(500);
        private static readonly ICollection<Guid> TransactionsToCleanup = new List<Guid>();
        private static readonly ICollection<Guid> UsersToCleanup = new List<Guid>();

        public static void AddTransactionToCleanup(Guid transactionId)
        {
            TransactionsToCleanup.Add(transactionId);
        }

        public static void AddUserIdToCleanup(Guid userId)
        {
            UsersToCleanup.Add(userId);
        }

        public static IEnumerable<Guid> GetTransactionsToCleanup()
        {
            return TransactionsToCleanup;
        }

        public static IEnumerable<Guid> GetUsersToCleanup()
        {
            return UsersToCleanup;
        }

        public static Guid GenerateTransactionId()
        {
            var id = Guid.NewGuid();
            AddTransactionToCleanup(id);
            return id;
        }

        public static Guid GenerateUserId()
        {
            var id = Guid.NewGuid();
            AddUserIdToCleanup(id);
            return id;
        }

        public static async Task<T> AssertWithRetry<T>(Func<Task<T>> func)
        {
            var retries = 0;
            do
            {
                try
                {
                    return await func().ConfigureAwait(false);
                }
                catch (AssertFailedException)
                {
                    await Task.Delay(DelayBetweenAssertRetries).ConfigureAwait(false);
                }

                retries++;
            }
            while (retries < NumberOfAssertRetries);

            Assert.Fail("Maximum number of assertion retries reached.");
            return default(T);
        }

        public static Task AssertWithRetry(Func<Task> func)
        {
            return AssertWithRetry<object>(
                () =>
                {
                    func();
                    return null;
                });
        }
    }
}