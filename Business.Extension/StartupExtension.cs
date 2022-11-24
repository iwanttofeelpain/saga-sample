using Business.API;
using Business.Engine;
using Business.Engine.Handlers;
using Business.Engine.StateMachine;
using Host.Extension.Mvc;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.ExtensionsDependencyInjectionIntegration.MultiBus;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Extension;

public static class StartupExtension
{
    private const string DefaultSectionName = "BusinessSettins";

    public static void AddBusinessService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(typeof(CreateBusinessProcessCommandHandler));
        services.AddEngine(configuration, DefaultSectionName);

        services.AddControllersFromAssemblies(typeof(BusinessController).Assembly);
    }

    public static IServiceCollectionBusConfigurator AddWorkStateMachine(
        this IServiceCollectionBusConfigurator configurator, IConfiguration configuration)
    {
        configurator.AddSagaStateMachine<BusinessStateMachine, BusinessState>()
            .MongoDbRepository(r =>
            {
                r.Connection = configuration.GetConnectionString("sagasPersistanceConnection");
                r.CollectionName = nameof(BusinessState);
            });
        return configurator;
    }
}