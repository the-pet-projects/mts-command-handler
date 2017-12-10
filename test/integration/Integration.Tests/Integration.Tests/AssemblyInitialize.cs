namespace Integration.Tests
{
    using Cassandra;
    using Configs;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AssemblyInitialize
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            foreach (var userId in Utils.GetUsersToCleanup())
            {
                CassandraConnection.Session.Execute(new SimpleStatement("DELETE FROM transactions WHERE user_id = ?", userId));
            }
        }
    }
}