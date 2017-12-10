namespace PetProjects.Mts.CommandHandler.Infrastructure.Configurations.DependencyInjection
{
    using Application.CommandHandlers;
    using Application.CommandHandlers.Transaction;
    using Domain.Model;
    using Framework.Cqrs.DependencyResolver;
    using Framework.Cqrs.Extensions.AspNetCore;
    using Framework.Cqrs.Mediator;
    using Infrasctructure.CrossCutting.Error;
    using Microsoft.Extensions.DependencyInjection;

    public static class ApplicationConfigurations
    {
        public static IServiceCollection LoadCommandHandlersConfigurations(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ISimpleMediator, SimpleMediator>();

            serviceCollection.AddCqrsDependencyResolver(sp =>
            {
                sp.RegisterCommandHandlerWithResponseAsync<CreateTransactionCommandHandlerAsync, CreateTransactionCommand, CommandResult<MicroTransaction>>(Lifetime.Transient);
            });

            return serviceCollection;
        }
    }
}