using BibliotecaAPI.Infrastructure.Messaging.Consumers;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Messaging;

public class MessageConsumerBackgroundService : BackgroundService
{
    private readonly UsuarioConsumer _usuarioConsumer;
    private readonly LivroConsumer _livroConsumer;
    private readonly EmprestimoConsumer _emprestimoConsumer;
    private readonly AdminConsumer _adminConsumer;

    public MessageConsumerBackgroundService(
        UsuarioConsumer usuarioConsumer,
        LivroConsumer livroConsumer,
        EmprestimoConsumer emprestimoConsumer,
        AdminConsumer adminConsumer)
    {
        _usuarioConsumer = usuarioConsumer;
        _livroConsumer = livroConsumer;
        _emprestimoConsumer = emprestimoConsumer;
        _adminConsumer = adminConsumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _usuarioConsumer.StartUsuarioConsuming();
        _livroConsumer.StartLivroConsuming(); 
        _emprestimoConsumer.StartEmprestimoConsuming();
        _adminConsumer.StartAdminConsuming();

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
