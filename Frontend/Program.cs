using Frontend.Components;
using Frontend.Models;
using Frontend.Servicos.Interfaces;
using Frontend.Servicos.Principal;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddScoped<ILogServico, LogServico>();
builder.Services.AddScoped<IValidacaoServico, ValidacaoServico>();
builder.Services.AddScoped<ILoginServico, LoginServico>();
builder.Services.AddScoped<IAmostraServico, AmostraServico>();
builder.Services.AddScoped<AvisoErro>();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/keys")) 
    .SetApplicationName("FrontEndKeys");

builder.WebHost.UseUrls("http://0.0.0.0:80"); //Utilizar essa linha para rodar a aplicação no docker

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("http://backend:80"); 
}); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();
app.UseStaticFiles();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
