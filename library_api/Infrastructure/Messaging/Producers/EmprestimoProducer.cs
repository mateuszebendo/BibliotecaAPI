using System.Text;
using System.Text.Json;
using library_api.Application.DTOs;
using RabbitMQ.Client;

namespace library_api.Infrastructure.Messaging.Producers;

public class EmprestimoProducer
{
    private readonly IModel _channel;

    public EmprestimoProducer(IModel channel)
    {
        _channel = channel;
    }
    
    public void EnviaAvisoEmprestimoRealizadoComSucesso(EmprestimoDTO emprestimo)
    {
        var json = JsonSerializer.Serialize(emprestimo);
        var body = Encoding.UTF8.GetBytes(json);
        _channel.BasicPublish(exchange: "emprestimo-alertas", routingKey: "emprestimo.criado", basicProperties: null, body: body);
    }
    
    public void EnviaAvisoEmprestimoFinalizado(EmprestimoDTO emprestimo)
    {
        var json = JsonSerializer.Serialize(emprestimo);
        var body = Encoding.UTF8.GetBytes(json);
        _channel.BasicPublish(exchange: "emprestimo-alertas", routingKey: "emprestimo.finalizado", basicProperties: null, body: body); }
}