using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using JJ.SmartHome.WebApi;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("devices.json", true, true);

builder.Services.AddControllers()
    .Services
    .ConfigureContainer(builder.Configuration);

var useSwagger = builder.Configuration.GetValue("Feature:Swagger", false);
if (useSwagger)
{
    builder.Services
        .AddEndpointsApiExplorer()
        .AddSwaggerGen();
}

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

if (useSwagger)
{
    app.UseSwagger()
        .UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();