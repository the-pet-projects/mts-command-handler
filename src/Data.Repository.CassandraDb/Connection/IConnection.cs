namespace PetProjects.Mts.CommandHandler.Data.Repository.CassandraDb.Connection
{
    using System;

    using Cassandra;
    using Cassandra.Mapping;

    public interface IConnection : IDisposable
    {
        ISession Session { get; }

        IMapper Mapper { get; }
    }
}