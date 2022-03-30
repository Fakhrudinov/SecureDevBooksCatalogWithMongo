using BooksRepositoryMongo;
using DataAbstraction.Interfaces;
using DataAbstraction.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connection to DataBase
builder.Services.Configure<BooksDataBaseConnection>(
    builder.Configuration.GetSection("BooksCatalogDataBaseConnectSettings"));

// Mongo repository 
builder.Services.AddTransient<IBooksRepository, BooksRepository>();

// connection to ElasticSearch
builder.Services.Configure<ElasticSearchConnection>(
    builder.Configuration.GetSection("ElasticSearch"));
// elastic service
builder.Services.AddSingleton<IElasticSearchService, ElasticSearchService.ElasticSearchService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
