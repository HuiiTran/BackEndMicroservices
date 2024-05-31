using Bill.Entities;
using Serilog;
using Serilog.Formatting.Json;
using ServicesCommon.MassTransit;
using ServicesCommon.MongoDB;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMongo()
    .AddMongoRepository<Bills>("Bills")
    .AddMongoRepository<CatalogItem>("CatalogItem")
    .AddMassTransitWithRabbitMq();

builder.Host.UseSerilog((ctx, config) =>
{
    config.WriteTo.Console().MinimumLevel.Information();
    config.WriteTo.File(
        path: "D:\\Bài làm các môn\\Mẫu thiết kế\\BackEnd\\Logs\\Bill\\BillLog-.txt",
        rollingInterval: RollingInterval.Day,
        rollOnFileSizeLimit: true,
        formatter: new JsonFormatter()).MinimumLevel.Information();
});

builder.Services.AddControllers();
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

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
