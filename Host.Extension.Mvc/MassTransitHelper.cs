using Hangfire;
using Host.Extension.Mvc.Settings;
using MassTransit;
using MassTransit.RabbitMqTransport;
using MassTransit.Registration;
using Microsoft.Extensions.Configuration;

namespace Host.Extension.Mvc;

public static class MassTransitHelper
{
    public static void UsingRabbitMqBus(
        this IBusRegistrationConfigurator configurator,
        IConfiguration configuration,
        Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>? configure = null)
    {
        RabbitSettings rabbitSettings = new RabbitSettings();
        ConfigurationBinder.Bind((IConfiguration)configuration.GetSection("RabbitSettings"), (object)rabbitSettings);
        configurator.UsingRabbitMq((Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>)((context, cfg) =>
        {
            cfg.Host(rabbitSettings.Uri, rabbitSettings.Port, rabbitSettings.VirtualHost, (Action<IRabbitMqHostConfigurator>)(h =>
            {
                h.Username(rabbitSettings.Username);
                h.Password(rabbitSettings.Password);
            }));
            cfg.Configure<IRabbitMqReceiveEndpointConfigurator>(context);
            configure?.Invoke(context, cfg);
        }));
    }

    private static void Configure<TReceiveEndpointConfigurator>(
        this IBusFactoryConfigurator<TReceiveEndpointConfigurator> cfg,
        IBusRegistrationContext context)
        where TReceiveEndpointConfigurator : IReceiveEndpointConfigurator
    {
        cfg.ConfigureEndpoints<TReceiveEndpointConfigurator>(context);
        cfg.UseInMemoryOutbox();
        cfg.UseHangfireScheduler(context, "hangfire", delegate (BackgroundJobServerOptions options)
        {
            options.WorkerCount = 1;
        });
    }
}