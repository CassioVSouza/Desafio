var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.WebHost.UseUrls("http://0.0.0.0:80");

app.MapGet("/", () => "Hello World!");

app.Run();
