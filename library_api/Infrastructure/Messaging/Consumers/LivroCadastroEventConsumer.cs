using library_api.Domain;
using MassTransit;

namespace library_api.Bus;

internal sealed class LivroCadastroEventConsumer : IConsumer<LivroCadastroEvent>
{
    private readonly ILogger<LivroCadastroEventConsumer> _logger;
    
    public LivroCadastroEventConsumer(ILogger<LivroCadastroEventConsumer> logger)
    {
        _logger = logger;
    }
    
    public async Task Consume(ConsumeContext<LivroCadastroEvent> request)
    {
        var livroCriado = request.Message;
        _logger.LogInformation("Criação do livro {nome} em processamento.", livroCriado.nome);

        await Task.Delay(10000);
        
        _logger.LogInformation("Criação do livro {nome} processada com sucesso.", livroCriado.nome);

    }
        
}