using ElasticCassandraExample.Application.Interfaces;
using ElasticCassandraExample.Application.Services;
using ElasticCassandraExample.Core.Interfaces;
using ElasticCassandraExample.Dal.Extensions;
using ElasticCassandraExample.Dal.Repositories;
using ElasticCassandraExample.ElasticSearch.Extension;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration.GetSection("ElasticSearchSettings");
builder.Services.AddElasticSearch(configuration["userName"], configuration["passWord"], configuration["defaultIndex"], configuration["uri"]);
builder.Services.AddRepositoryExtensions();
builder.Services.AddCassandra(builder.Configuration);


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
