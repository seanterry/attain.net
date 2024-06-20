using Attain;
using Attain.Components;

var builder = WebApplication.CreateBuilder( args );

// local development services
if ( builder.Environment.IsDevelopment() )
{
    builder.Configuration.AddUserSecrets<Program>();
}

// deployed services
else
{
    builder.AddSystemsManagerSecrets();

    builder.Services.AddHsts( options =>
    {
        options.MaxAge = TimeSpan.FromDays( 365 );
        options.IncludeSubDomains = true;
    } );
}

builder.AddAppDbContext();

builder.Services.AddRazorComponents();

var app = builder.Build();

// deployed application settings
if ( !app.Environment.IsDevelopment() )
{
    app.UseExceptionHandler( "/Error", createScopeForErrors: true );
    app.UseHsts();
}

app.UseStaticFiles( new StaticFileOptions
{
    // instruct the browser to validate the cached item before re-using it
    OnPrepareResponse = ctx => ctx.Context.Response.Headers["Cache-Control"] = "public,no-cache"
} );

app.UseAntiforgery();

app.MapRazorComponents<App>();

await app.RunAsync();
