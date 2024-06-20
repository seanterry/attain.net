using Attain;

var builder = Host.CreateApplicationBuilder( args );

if ( builder.Environment.IsDevelopment() )
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
