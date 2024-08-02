using System.Text;
using System.Text.Json;
using BibliotecaAPI.Application.Entities;
using BibliotecaAPI.Domain.Messaging.Producers;
using RabbitMQ.Client;

namespace BibliotecaAPI.Infrastructure.Messaging.Producers;

public class EmprestimoProducer : IEmprestimoProducer
{
    private readonly IModel _channel;

    public EmprestimoProducer(IModel channel)
    {
        _channel = channel;
    }
    
    public void EnviaAvisoEmprestimoRealizadoComSucesso(Emprestimo emprestimo)
    {
        var json = JsonSerializer.Serialize(emprestimo);
        var body = Encoding.UTF8.GetBytes(json);
        _channel.BasicPublish(exchange: "emprestimo-alertas", routingKey: "emprestimo.criado", basicProperties: null, body: body);
    }
    
    public void EnviaAvisoEmprestimoFinalizado(Emprestimo emprestimo)
    {
        var json = JsonSerializer.Serialize(emprestimo);
        var body = Encoding.UTF8.GetBytes(json);
        _channel.BasicPublish(exchange: "emprestimo-alertas", routingKey: "emprestimo.finalizado", basicProperties: null, body: body); }
}