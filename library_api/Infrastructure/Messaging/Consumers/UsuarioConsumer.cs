using System.Text;
using System.Text.Json;
using library_api.Application.DTOs;
using library_api.Domain.Enums;
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
        var usuarioAlteradoConsumer = new EventingBasicConsumer(_channel);
        var usuarioCriadoConsumer = new EventingBasicConsumer(_channel);

        usuarioAlteradoConsumer.Received += (sender, eventArgs) =>
        {
            try
            {
                var body = eventArgs.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var usuario = JsonSerializer.Deserialize<UsuarioDTO>(json);
            
                if (usuario != null)
                {
                    if (usuario.Status != StatusUsuario.Ativo)
                    {
                        ProcessarUsuarioBloqueadoMensagem(usuario);
                        return;
                    }
                    ProcessarUsuarioAlteradoMensagem(usuario);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar mensagem: {ex.Message}");
            }
        };

        usuarioCriadoConsumer.Received += (sender, eventArgs) =>
        {
            try
            {
                var body = eventArgs.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var usuario = JsonSerializer.Deserialize<UsuarioDTO>(json);
            
                if (usuario != null)
                {
                    ProcessarUsuarioCriadoMensagem(usuario);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar mensagem: {ex.Message}");
            }
        };

        _channel.BasicConsume(queue: "usuarios-alterado", autoAck: true, consumer: usuarioAlteradoConsumer);
        _channel.BasicConsume(queue: "usuario-criado", autoAck: true, consumer: usuarioCriadoConsumer);
    }


    private void ProcessarUsuarioBloqueadoMensagem(UsuarioDTO usuario)
    {
        Console.WriteLine($"====================================================================================\n" + 
                          $"MENSAGEM PARA {usuario.Email.ToUpper()}: \nSeu usuário '{usuario.Nome}' esta {usuario.Status.ToString().ToUpper()} por tempo indeterminado! \nVisite nosso site para saber como reverter isso.\n");
    }
    
    private void ProcessarUsuarioAlteradoMensagem(UsuarioDTO usuario)
    {
        Console.WriteLine($"====================================================================================\n" +
                          $"MENSAGEM PARA {usuario.Email}: \nUsuário '{usuario.Nome}' foi alterado com sucesso!\n");
    }
    
    private void ProcessarUsuarioCriadoMensagem(UsuarioDTO usuario)
    {
        Console.WriteLine($"====================================================================================\n" +
                          $"USUÁRIO '{usuario.Nome.ToUpper()}' CRIADO COM SUCESSO ! Confirme no seu email '{usuario.Email}'\n");
    }
}