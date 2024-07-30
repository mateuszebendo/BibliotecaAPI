using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace library_api.Infrastructure.Messaging.Consumers;

public class UsuarioConsumer
{
    private readonly IModel _channel;

    public UsuarioConsumer(IModel channel)
    {
        _channel = channel;
    }

    public void StartUsuarioConsuming()
    {
        var usuarioBloqueadoConsumer = new EventingBasicConsumer(_channel);
        
        usuarioBloqueadoConsumer.Received += (sender, eventArgs) =>
        {
            byte[] body = eventArgs.Body.ToArray();
            string mensagem = Encoding.UTF8.GetString(body);
            ProcessarUsuarioBloqueadoMensagem(mensagem);
        };

        _channel.BasicConsume(queue: "usuarios-bloqueados", autoAck: true, consumer: usuarioBloqueadoConsumer);
    }

    private void ProcessarUsuarioBloqueadoMensagem(string usuario)
    {
        Console.WriteLine($"O usuário {usuario} foi bloqueado por tempo indeterminado!");
    }
}