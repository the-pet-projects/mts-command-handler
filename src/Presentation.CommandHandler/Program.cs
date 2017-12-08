﻿namespace PetProjects.Mts.CommandHandler.Presentation.ConsoleApplication
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Configurations;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    internal class Program
    {
        private static readonly ManualResetEvent QuitEvent = new ManualResetEvent(false);

        private static void Main(string[] args)
        {
            // Init Bootstrapper with appsettings
            var configuration = new Bootstrapper()
                                    .BootstrapContainer();

            using (var parentServiceProvider = configuration.ServiceCollection.BuildServiceProvider())
            {
                Program.Run(parentServiceProvider);
            }
        }

        private static void Run(IServiceProvider scopedProvider)
        {
            var logger = scopedProvider.GetService<ILoggerFactory>().CreateLogger<Program>();

            logger.LogCritical("Starting Mts CommandHandler...");

            //using (var consumer = scopedProvider.GetRequiredService<IConsumer<TransactionCommand>>())
            //{
            //    consumer.StartConsuming();

                Console.CancelKeyPress += (sender, eArgs) =>
                {
                    Program.QuitEvent.Set();
                    eArgs.Cancel = true;
                };

                Program.QuitEvent.WaitOne();

                //logger.LogWarning("Received notification to exit. Stopping and disposing consumer...");

                //consumer.Dispose();
            //}

            logger.LogCritical("Mts.CommandHandler Ended...");

            // wait 2 seconds for previous log to reach the sink
            Task.Delay(TimeSpan.FromMilliseconds(2000)).Wait();
        }
    }
}