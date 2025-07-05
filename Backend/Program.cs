using Backend.Data;
using Backend.Repositorio.Interfaces;
using Backend.Repositorio.Principal;
using Backend.Servicos.Interfaces;
using Backend.Servicos.Principal;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:80"); //Utilize essa URL para ser possível utilizar o localhost do docker

builder.Services.AddScoped<ILogServico, LogServico>();
builder.Services.AddScoped<IAmostraRepositorio, AmostraRepositorio>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<SqlContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
