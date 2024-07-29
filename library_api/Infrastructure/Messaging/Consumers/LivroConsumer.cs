using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace library_api.Infrastructure.Messaging.Consumers;

public class LivroConsumer
{
    private readonly IModel _channel;

    public LivroConsumer(IModel channel)
    {
        _channel = channel;
    }

    public void StartConsuming()
    {
        var consumer = new EventingBasicConsumer(_channel);
        
        consumer.Received += (sender, eventArgs) =>
        {
            byte[] body = eventArgs.Body.ToArray();
            string mensagem = Encoding.UTF8.GetString(body);
            ProcessarMensagem(mensagem);
        };

        _channel.BasicConsume(queue: "novos_livros_queue", autoAck: true, consumer: consumer);
    }

    private void ProcessarMensagem(string mensagem)
    {
        Console.WriteLine($"Mensagem recebida: {mensagem}");
    }
}