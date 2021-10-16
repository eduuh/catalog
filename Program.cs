using catalog.Repositories;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Catalog.Settings;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Net.Mime;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var settings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();


builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "catalog", Version = "v1" });
});

builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddSingleton<IInMemItemsRepository, MongoDbItemsRepository>();
builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

builder.Services.AddHealthChecks() 
	         .AddMongoDb(settings.ConnectionString, name: "mongodb", timeout: TimeSpan.FromSeconds(3), tags: new []{ "ready"});

BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "catalog v1"));
}
app.UseHttpsRedirection();


app.UseAuthorization();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHealthChecks("health/ready", new HealthCheckOptions{
             Predicate = (check) => check.Tags.Contains("ready"),
	     ResponseWriter = async(context, report) => {
	         var result = JsonSerializer.Serialize( new {
                                   status = report.Status.ToString(),
				   check = report.Entries.Select(entry => new {
                                                     name= entry.Key,
						     exception = entry.Value.Exception is not null ?entry.Value.Exception.Message : "none",
						     duration= entry.Value.Duration.ToString()
						   })
				 });
		 context.Response.ContentType = MediaTypeNames.Application.Json;
		 await context.Response.WriteAsync(result);
	     }
		    });
    endpoints.MapHealthChecks("health/live", new HealthCheckOptions{
             Predicate = (_) => false
		    });
});

app.Run();
