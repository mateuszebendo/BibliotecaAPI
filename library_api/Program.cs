using library_api.Application.Interfaces;
using library_api.Domain.DomainInterfaces;
using library_api.Domain.Enums;
using library_api.Infrastructure.Messaging;
using library_api.Domain.Repositories;
using library_api.Domain.Services;
using library_api.Infrastructure.Data;
using library_api.Infrastructure.DataBase;
using library_api.Infrastructure.Messaging.Consumers;
using library_api.Infrastructure.Messaging.Producers;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

DatabaseConfig.Initialize(configuration);

builder.Configuration.AddJsonFile("rabbitmqsettings.json", optional: true, reloadOnChange: true);

builder.Services.AddControllers();

builder.Services.AddScoped<ILivroRepository, LivroRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IEmprestimoRepository, EmprestimoRepository>();

builder.Services.AddScoped<ILivroService, LivroService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IEmprestimoService, EmprestimoService>();

builder.Services.AddScoped<IEmprestimoDomainService, EmprestimoDomainService>();
builder.Services.AddScoped<ILivroDomainService, LivroDomainService>();

builder.Services.AddSingleton<LivroProducer>();
builder.Services.AddSingleton<LivroConsumer>();
builder.Services.AddSingleton<EmprestimoProducer>();
builder.Services.AddSingleton<EmprestimoConsumer>();
builder.Services.AddSingleton<UsuarioProducer>();
builder.Services.AddSingleton<UsuarioConsumer>();
builder.Services.AddSingleton<AdminConsumer>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRabbitMQServices(configuration);

builder.Services.AddHostedService<BackgroundMessageConsumer>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();