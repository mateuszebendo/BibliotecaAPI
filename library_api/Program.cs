using library_api.Domain.Enums;
using library_api.Infrastructure.Messaging;
using library_api.Domain.Repositories;
using library_api.Domain.Services;
using library_api.Infrastructure.Data;
using library_api.Infrastructure.DataBase;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

DatabaseConfig.Initialize(configuration);
builder.Services.AddControllers();
builder.Services.AddScoped<ILivroRepository, LivroRepository>();
builder.Services.AddScoped<LivroService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRabbitMQService();
NpgsqlConnection.GlobalTypeMapper.MapEnum<StatusLivro>("disponibilidade_enum");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
