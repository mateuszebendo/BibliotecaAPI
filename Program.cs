// using library_api.Infrastructure.Messaging;

using library_api.Domain.Repositories;
using library_api.presentation.controllers;
using library_api.Infrastructure.Data;
using library_api.Infrastructure.DataBase;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

DatabaseConfig.Initialize(configuration);
builder.Services.AddControllers();
builder.Services.AddScoped<ILivroRepository, LivroRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddRabbitMQService();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
