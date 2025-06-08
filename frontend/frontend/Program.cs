using frontend.Components;
using Microsoft.AspNetCore.Components.Authorization;
using frontend.Providers; // <- заменить на ваш namespace
using frontend.Services;   // <- AuthService
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "Custom";
        options.DefaultSignInScheme = "Custom";
        options.DefaultChallengeScheme = "Custom";
    })
    .AddCookie("Custom", options =>
    {
        options.LoginPath = "/auth/login"; // необязательно, но можно указать
    });
builder.Services.AddAuthorizationCore();

builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("https://storage-manager-backend.fiwka.xyz/"); 
});

builder.Services.AddScoped<AuthenticationStateProvider, AuthProvider>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddMudServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();   // <-- ВАЖНО
app.UseAuthorization();    // <-- ВАЖНО

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(frontend.Client._Imports).Assembly);

app.Run();