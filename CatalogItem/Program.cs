using CatalogItem.Entities;
using JWTAuthenManager;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ServicesCommon.MassTransit;
using ServicesCommon.MongoDB;
using System.Text;


var builder = WebApplication.CreateBuilder(args);



builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCustomJwtAuthentication();

builder.Services.AddMongo()
    .AddMongoRepository<Laptop>("laptops")
    .AddMassTransitWithRabbitMq();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapHealthChecks("health");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();
