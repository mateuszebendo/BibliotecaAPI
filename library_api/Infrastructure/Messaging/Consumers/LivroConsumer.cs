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

    public void StartLivroConsuming()
    {
        var livroDisponivelConsumer = new EventingBasicConsumer(_channel);
        var livroLancadoConsumer = new EventingBasicConsumer(_channel);

        
        livroDisponivelConsumer.Received += (sender, eventArgs) =>
        {
            byte[] body = eventArgs.Body.ToArray();
            string mensagem = Encoding.UTF8.GetString(body);
            ProcessaLivroDisponivelMensagem(mensagem);
        };

        livroLancadoConsumer.Received += (sender, eventArgs) =>
        {
            byte[] body = eventArgs.Body.ToArray();
            string mensagem = Encoding.UTF8.GetString(body);
            ProcessaLivroLancadoMensagem(mensagem);
        };

        _channel.BasicConsume(queue: "livro-disponivel", autoAck: true, consumer: livroDisponivelConsumer);
        _channel.BasicConsume(queue: "livro-lancado", autoAck: true, consumer: livroLancadoConsumer);

    }

    private void ProcessaLivroDisponivelMensagem(string mensagem)
    {
        Console.WriteLine($"Um exemplar de {mensagem}, acaba de ficar disponivel!");
    }
    
    private void ProcessaLivroLancadoMensagem(string mensagem)
    {
        Console.WriteLine($"Aproveite o ultimo lançamento da Biblioteca do Astolfo: '{mensagem}' já nas pratileiras!");
    }
}