using Attain;
using Attain.Components;
using Attain.Components.Branding;

var builder = WebApplication.CreateBuilder( args );

// local development services
if ( builder.Environment.IsDevelopment() )
{
    builder.Configuration.AddEnvironmentVariables( "ATTAIN_" );
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

builder.Services.Scan( scan => scan
    .FromAssemblies( typeof(Program).Assembly )
    .AddClasses( classes => classes.AssignableTo<ITransient>() ).AsSelf().AsImplementedInterfaces().WithTransientLifetime()
    .AddClasses( classes => classes.AssignableTo<IScoped>() ).AsSelf().AsImplementedInterfaces().WithScopedLifetime()
    .AddClasses( classes => classes.AssignableTo<ISingleton>() ).AsSelf().AsImplementedInterfaces().WithSingletonLifetime()
);

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

app.UseMiddleware<BrandingMiddleware>();
app.UseAntiforgery();

app.MapRazorComponents<App>();

await app.RunAsync();
