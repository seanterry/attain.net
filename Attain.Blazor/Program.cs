using Attain.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents();

if ( !builder.Environment.IsDevelopment() )
{
    builder.Services.AddHsts( options =>
    {
        options.MaxAge = TimeSpan.FromDays( 365 );
        options.IncludeSubDomains = true;
    } );
}

var app = builder.Build();

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

app.Run();
