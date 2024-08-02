using System.Text;
using System.Text.Json;
using BibliotecaAPI.Application.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ArgumentException = System.ArgumentException;

namespace BibliotecaAPI.Infrastructure.Messaging.Consumers;

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
            var json = Encoding.UTF8.GetString(body);
            Livro livro = JsonSerializer.Deserialize<Livro>(json);
            ProcessaLivroDisponivelMensagem(livro);
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

    private async void ProcessaLivroDisponivelMensagem(Livro livro)
    {
        try
        {
            
            Console.WriteLine($"====================================================================================\n" + 
                              $"Um exemplar de {livro.Nome}, acaba de ficar disponivel! \n" +
                              $"Reserve agora mesmo. \nId do livro: {livro.LivroId}\n");
        }
        catch (ArgumentException error)
        {
            Console.WriteLine("Um erro inesperado aconteceu no processamento de um livro: " + error);
        }
    }
    
    private void ProcessaLivroLancadoMensagem(string nomeLivro)
    {
        Console.WriteLine($"====================================================================================\n" + 
                          $"Aproveite o ultimo lançamento da Biblioteca do Astolfo: '{nomeLivro}' já nas pratileiras!\n");
    }
}