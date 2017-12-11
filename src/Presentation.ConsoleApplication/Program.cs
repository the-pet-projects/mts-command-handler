namespace PetProjects.Mts.CommandHandler.Presentation.ConsoleApplication
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Configurations;
    using Framework.Kafka.Consumer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using MicroTransactions.Commands.Transactions.V1;

    internal class Program
    {
        private static readonly ManualResetEvent QuitEvent = new ManualResetEvent(false);

        private static void Main(string[] args)
        {
            // Init Bootstrapper with appsettings
            var configuration = new Bootstrapper()
                                    .BootstrapContainer();

            using (var rootProvider = configuration.ServiceCollection.BuildServiceProvider())
            {
                using (var scopedProvider = rootProvider.CreateScope())
                {
                    Program.Run(scopedProvider.ServiceProvider);
                }
            }
        }

        private static void Run(IServiceProvider scopedProvider)
        {
            var logger = scopedProvider.GetService<ILoggerFactory>().CreateLogger<Program>();

            logger.LogCritical("Starting Mts CommandHandler...");

            try
            {
                using (var consumer = scopedProvider.GetRequiredService<IConsumer<TransactionCommandV1>>())
                {
                    consumer.StartConsuming();

                    Console.CancelKeyPress += (sender, eArgs) =>
                    {
                        Program.QuitEvent.Set();
                        eArgs.Cancel = true;
                    };

                    Program.QuitEvent.WaitOne();
                }
            }
            catch (Exception exception)
            {
                logger.LogCritical(exception, "Fatal Exception occured.");
            }

            logger.LogCritical("Mts.CommandHandler Ended...");

            // wait 2 seconds for previous log to reach the sink
            Task.Delay(TimeSpan.FromMilliseconds(2000)).Wait();
        }
    }
}