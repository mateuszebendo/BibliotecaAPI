// using library_api.Relatorios;
// using MassTransit;
//
// namespace library_api.Bus;
//
// internal sealed class LivroSolicitadoEventConsumer : IConsumer<RelatorioSolicitadoEvent>
// {
//     private readonly ILogger<LivroSolicitadoEventConsumer> _logger;
//
//     public LivroSolicitadoEventConsumer(ILogger<LivroSolicitadoEventConsumer> logger)
//     {
//         _logger = logger;
//     }
//     
//     public async Task Consume(ConsumeContext<RelatorioSolicitadoEvent> context)
//     {
//         var message = context.Message;
//         _logger.LogInformation("Processando Relatório Id: {Id}, Nome {Nome}", message.Id, message.Name);
//
//         await Task.Delay(10000);
//
//         var relatorio = Lista.Relatorios.FirstOrDefault(x => x.Id == message.Id);
//         if (relatorio != null)
//         {
//             relatorio.Status = "Completado!";
//             relatorio.ProcessedTime = DateTime.Now;
//         }
//         _logger.LogInformation("Relatório Processado! Id: {Id}, Nome {Nome}", message.Id, message.Name);
//     }
// }