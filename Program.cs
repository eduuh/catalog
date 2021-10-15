using catalog.Repositories;
using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "catalog", Version = "v1" });
});
builder.Services.AddSingleton<IInMemItemsRepository, InMemItemsRepository>();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "catalog v1"));
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
