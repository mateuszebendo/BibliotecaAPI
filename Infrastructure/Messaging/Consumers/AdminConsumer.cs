using System.Text;
using System.Text.Json;
using BibliotecaAPI.Application.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BibliotecaAPI.Infrastructure.Messaging.Consumers;

public class AdminConsumer
{
    private readonly IModel _channel;
    
    public AdminConsumer(IModel channel)
    {
        _channel = channel;
    }
    
    public void StartAdminConsuming()
    {
        var adminUsuarioAlertas = new EventingBasicConsumer(_channel);
    
        adminUsuarioAlertas.Received += (sender, eventArgs) =>
        {
            try
            {
                var body = eventArgs.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var usuario = JsonSerializer.Deserialize<Usuario>(json);

                if (usuario != null)
                {
                    ProcessarUsuarioMensagem(usuario);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar mensagem: {ex.Message}");
            }
        };

        _channel.BasicConsume(queue: "alerta-admin-usuario", autoAck: true, consumer: adminUsuarioAlertas);
    }


    private void ProcessarUsuarioMensagem(Usuario usuario)
    {
        Console.WriteLine($"====================================================================================\n" + 
                          $"AVISO ADMINISTRATIVO:\nO usuário '{usuario.Nome.ToUpper()}' de Id {usuario.UsuarioId} foi alterado/criado!\n");
    }
}