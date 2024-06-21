using Attain;

var builder = Host.CreateApplicationBuilder( args );

builder.AddSystemsManagerSecrets();
builder.Services.AddConventionalServices();
builder.AddAppDbContext();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
