using Microsoft.AspNetCore.Builder;
using ProxyApp.Handlers;
using ProxyApp.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Get the endpoint configuration from the appsettings.json
builder.Services.Configure<List<ProxyApp.Model.Endpoint>>(builder.Configuration.GetSection("Endpoints"));

var app = builder.Build();

// Configure the HTTP request pipeline.
List<ProxyApp.Model.Endpoint> endpoints = builder.Configuration.GetSection("Endpoints").Get<List<ProxyApp.Model.Endpoint>>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = "";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Default");

        foreach (var endpoint in endpoints)
            c.SwaggerEndpoint(endpoint.ProxyUrlPath, endpoint.Name);
    });
}

// Add the interceptor middleware in the configuration of the app
app.UseMiddleware<InterceptorHandler>();
app.UseAuthorization();
app.MapControllers();
app.Run();
