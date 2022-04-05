using BooksCatalog.ConsulService;
using BooksRepositoryMongo;
using Consul;
using DataAbstraction.Interfaces;
using DataAbstraction.Models;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Net.Mime;
using System.Reflection;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// configuring ELC
builder.Host
		.ConfigureAppConfiguration(configuration =>
		{
			configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
			configuration.AddJsonFile(
				$"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
				optional: true);
		})
		.UseSerilog();
ConfigureLogging();


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

//consul
builder.Services.AddSingleton<IHostedService, ConsulHostedService>();
builder.Services.Configure<ConsulConfiguration>(builder.Configuration.GetSection("consulConfiguration"));
builder.Services.AddSingleton<IConsulClient, ConsulClient>(p =>
    new ConsulClient(consulConfig =>
        {
            var address = builder.Configuration["consulConfiguration:address"];
            consulConfig.Address = new Uri(address);
        }));
builder.Services.AddHealthChecks();
//consul KeyValue store == without cancellation token
builder.Services.AddSingleton <Func<IConsulClient>> (p => () => 
	new ConsulClient(consulConfig =>
        {
            var address = builder.Configuration["consulConfiguration:address"];
            consulConfig.Address = new Uri(address);
        }));


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

//consul
app.MapHealthChecks("/healthz");
app.UseHealthChecks("/healthz");
app.MapHealthChecks("/healthcheck-details",
	new HealthCheckOptions
	{
		ResponseWriter = async (context, report) =>
		{
			var result = JsonSerializer.Serialize(
				new
				{
					status = report.Status.ToString(),
					monitors = report.Entries.Select(e => new { key = e.Key, value = Enum.GetName(typeof(HealthStatus), e.Value.Status) })
				});
			context.Response.ContentType = MediaTypeNames.Application.Json;
			await context.Response.WriteAsync(result);
		}
	}
);


Log.Warning($"Program started {Assembly.GetExecutingAssembly().GetName().Name}");
app.Run();


static void ConfigureLogging()
{
	var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
	var configuration = new ConfigurationBuilder()
		.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
		.AddJsonFile(
			$"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
			optional: true)
		.Build();

	Log.Logger = new LoggerConfiguration()
		.Enrich.FromLogContext()
		.Enrich.WithMachineName()
		.WriteTo.Debug()
		.WriteTo.Console()
		.WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
		.Enrich.WithProperty("Environment", environment)
		.ReadFrom.Configuration(configuration)
		.CreateLogger();
}

static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
{
	return new ElasticsearchSinkOptions(new Uri(configuration["ElasticSearch:Uri"]))
	{
		AutoRegisterTemplate = true,
		IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-logs"
	};
}
