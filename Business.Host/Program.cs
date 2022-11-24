using Business.BW.Consumers;
using Business.Extension;
using Hangfire;
using Hangfire.PostgreSql;
using Host.Extension.Mvc;
using MassTransit;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumersFromNamespaceContaining<CreateConsumer>();
    configurator.UsingRabbitMqBus(builder.Configuration);
    configurator.AddMessageScheduler(new Uri("queue:hangfire"));
    configurator.AddWorkStateMachine(builder.Configuration);
});
builder.Services.AddBusinessService(builder.Configuration);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Business API", Version = "v1" });
    c.EnableAnnotations();
    string[] files = Directory.GetFiles(AppContext.BaseDirectory, "*.API*.xml", SearchOption.TopDirectoryOnly);
    foreach (string filePath in files)
    {
        c.IncludeXmlComments(filePath, includeControllerXmlComments: true);
    }
});

builder.Services.AddSwaggerGenNewtonsoftSupport();

var hangfireConnectionString = builder.Configuration.GetConnectionString("postgresConnection");

builder.Services.AddHangfire(c => c
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(hangfireConnectionString, new PostgreSqlStorageOptions
    {
        SchemaName = "monolith-hangfire",
        DistributedLockTimeout = TimeSpan.FromMinutes(1),
        InvisibilityTimeout = TimeSpan.FromMinutes(5),
        UseNativeDatabaseTransactions = true
    })
);
builder.Services.AddHangfireServer(e => e.WorkerCount = 1);
builder.Services.AddMassTransitHostedService();

var app = builder.Build();
app.UseHangfireDashboard();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
});

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();