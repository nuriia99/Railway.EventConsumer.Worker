using Railway.EventConsumer.Application.Configuration;
using Railway.EventConsumer.Infrastructure.Configuration;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var host = builder.Build();
host.Run();
