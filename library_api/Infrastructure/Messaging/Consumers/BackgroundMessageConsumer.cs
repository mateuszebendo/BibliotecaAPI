using library_api.Domain.Services;

namespace library_api.Infrastructure.Messaging.Consumers;

public class BackgroundMessageConsumer  : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public BackgroundMessageConsumer(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var livroService = scope.ServiceProvider.GetRequiredService<LivroService>();
                livroService.IniciarConsumo();
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Ajuste o intervalo conforme necessário
        }
    }
}