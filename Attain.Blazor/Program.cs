using Attain.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles( new StaticFileOptions
{
    // instruct the browser to validate the cached item before re-using it
    OnPrepareResponse = ctx => ctx.Context.Response.Headers["Cache-Control"] = "public,no-cache"
} );

app.UseAntiforgery();

app.MapRazorComponents<App>();

app.Run();
