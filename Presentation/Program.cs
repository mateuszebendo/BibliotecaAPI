using BibliotecaAPI.Application.Interfaces;
using BibliotecaAPI.Application.Services;
using BibliotecaAPI.Domain.DomainInterfaces;
using BibliotecaAPI.Domain.Messaging.Producers;
using BibliotecaAPI.Infrastructure.Messaging.Consumers;
using BibliotecaAPI.Domain.Repositories;
using BibliotecaAPI.Domain.Services;
using BibliotecaAPI.Infrastructure.Data;
using BibliotecaAPI.Infrastructure.DataBase;
using BibliotecaAPI.Infrastructure.Messaging;
using BibliotecaAPI.Infrastructure.Messaging.Producers;
using Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Configuration.AddJsonFile("rabbitmqsettings.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

DatabaseConfig.Initialize(configuration);

builder.Services.AddScoped<IEmprestimoRepository, EmprestimoRepository>(); 
builder.Services.AddScoped<ILivroRepository, LivroRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddScoped<IEmprestimoDomainService, EmprestimoDomainService>();
builder.Services.AddScoped<ILivroDomainService, LivroDomainService>();

builder.Services.AddScoped<ILivroService, LivroService>();
builder.Services.AddScoped<IEmprestimoService, EmprestimoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();


builder.Services.AddScoped<ILivroProducer, LivroProducer>();
builder.Services.AddScoped<IEmprestimoProducer, EmprestimoProducer>();
builder.Services.AddScoped<IUsuarioProducer, UsuarioProducer>();

builder.Services.AddControllers();

builder.Services.AddSingleton<UsuarioConsumer>();
builder.Services.AddSingleton<LivroConsumer>();
builder.Services.AddSingleton<EmprestimoConsumer>();
builder.Services.AddSingleton<AdminConsumer>();

builder.Services.AddRabbitMQServices(configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<MessageConsumerBackgroundService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
