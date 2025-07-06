using Frontend.Components;
using Frontend.Models;
using Frontend.Servicos.Interfaces;
using Frontend.Servicos.Principal;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddScoped<ILogServico, LogServico>();
builder.Services.AddScoped<IValidacaoServico, ValidacaoServico>();
builder.Services.AddScoped<ILoginServico, LoginServico>();


//builder.WebHost.UseUrls("http://0.0.0.0:80"); //Utilizar essa linha para rodar a aplicação no docker

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5001"); 
}).AddHttpMessageHandler<AuthHeaderServico>(); //Adiciona o Token automaticamente ao fazer uma requisição

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
