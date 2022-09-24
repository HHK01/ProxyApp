using Microsoft.AspNetCore.Builder;
using ProxyApp.Handlers;
using ProxyApp.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EXPLAIN HERE,EXPLAIN HERE,EXPLAIN HERE,EXPLAIN HERE,EXPLAIN HERE,EXPLAIN HERE
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

// EXPLAIN HERE,EXPLAIN HERE,EXPLAIN HERE,EXPLAIN HERE,EXPLAIN HERE,EXPLAIN HERE
app.UseMiddleware<InterceptorHandler>();
app.UseAuthorization();
app.MapControllers();
app.Run();
