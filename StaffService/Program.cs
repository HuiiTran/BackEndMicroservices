using ServicesCommon.MassTransit;
using ServicesCommon.MongoDB;
using StaffService.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddMongo()
    .AddMongoRepository<Staff>("Staff")
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

app.UseAuthorization();

app.MapControllers();

app.Run();
