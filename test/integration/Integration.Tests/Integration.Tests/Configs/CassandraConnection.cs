﻿namespace Integration.Tests.Configs
{
    using System;
    using Cassandra;

    internal static class CassandraConnection
    {
        private static readonly Lazy<ISession> LazySession = BuildSession();

        public static ISession Session => LazySession.Value;

        private static Lazy<ISession> BuildSession()
        {
            return new Lazy<ISession>(() =>
            {
                var cluster = Cluster.Builder()
                    .AddContactPoints(AppSettings.Current.CassandraContactPoints.Split(','))
                    .WithDefaultKeyspace(AppSettings.Current.CassandraKeyspace)
                    .Build();

                return cluster.Connect();
            });
        }
    }
}