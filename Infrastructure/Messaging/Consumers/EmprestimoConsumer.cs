using System.Text;
using System.Text.Json;
using BibliotecaAPI.Application.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BibliotecaAPI.Infrastructure.Messaging.Consumers;

public class EmprestimoConsumer
{
    private readonly IModel _channel;

    public EmprestimoConsumer(IModel channel)
    {
        _channel = channel;
    }

    public void StartEmprestimoConsuming()
    {
        var emprestimoCriadoConsumer = new EventingBasicConsumer(_channel);
        var emprestimoFinalizadoConsumer = new EventingBasicConsumer(_channel);

        emprestimoCriadoConsumer.Received += (sender, eventArgs) =>
        {
            try
            {
                var body = eventArgs.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var emprestimo = JsonSerializer.Deserialize<Emprestimo>(json);
            
                if (emprestimo != null)
                {
                    ProcessarEmprestimoCriadoMensagem(emprestimo);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar mensagem: {ex.Message}");
            }
        };

        emprestimoFinalizadoConsumer.Received += (sender, eventArgs) =>
        {
            try
            {
                var body = eventArgs.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var emprestimo = JsonSerializer.Deserialize<Emprestimo>(json);
            
                if (emprestimo != null)
                {
                    ProcessarEmprestimoFinalizadoMensagem(emprestimo);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar mensagem: {ex.Message}");
            }
        };

        _channel.BasicConsume(queue: "emprestimo-criado", autoAck: true, consumer: emprestimoCriadoConsumer);
        _channel.BasicConsume(queue: "emprestimo-finalizado", autoAck: true, consumer: emprestimoFinalizadoConsumer);
    }


    private void ProcessarEmprestimoCriadoMensagem(Emprestimo emprestimoDto)
    {
        Console.WriteLine($"====================================================================================\n" + 
                          $"Usuário de Id {emprestimoDto.UsuarioId} realizou um emprestimo do livro de Id {emprestimoDto.LivroId}. \nData de entrega: {emprestimoDto.DataDevolucao}\n");
    }
    
    private void ProcessarEmprestimoFinalizadoMensagem(Emprestimo emprestimoDto)
    {
        Console.WriteLine($"====================================================================================\n" +
                          $"Empréstimo de Id {emprestimoDto.EmprestimoId} finalizado com status {emprestimoDto.Status.ToString().ToUpper()}\n");
    }
}