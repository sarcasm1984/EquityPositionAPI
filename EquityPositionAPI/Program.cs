//using System.Transactions;

using EquityPositionAPI.Models;
using EquityPositionAPI.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<TrafTestDatabaseSettings>(builder.Configuration.GetSection(nameof(TrafTestDatabaseSettings)));
builder.Services.AddSingleton<ITrafTestDatabaseSettings>(t => t.GetRequiredService<IOptions<TrafTestDatabaseSettings>>().Value);
builder.Services.AddSingleton<IMongoClient>(s => new MongoClient(builder.Configuration.GetValue<string>("TrafTestDatabaseSettings:ConnectionString")));
builder.Services.AddScoped<ITransactionPosition, TransactionPositionService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddScoped<Transaction>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.Run();

