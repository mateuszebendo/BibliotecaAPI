// using library_api.Bus;
// using MassTransit;
//
// namespace library_api.Infrastructure.Messaging;
//
// internal static class RabbitMQConfig
// {
//     public static void AddRabbitMQService(this IServiceCollection services)
//     {
//         services.AddMassTransit(busConfigurator =>
//         {
//             busConfigurator.AddConsumer<LivroSolicitadoEventConsumer>();
//             
//             busConfigurator.UsingRabbitMq((ctx, cfg) =>
//             {
//                 cfg.Host(new Uri("amqp://localhost:5672"), host =>
//                 {
//                     host.Username("guest");
//                     host.Password("guest");
//                 });
//                 
//                 cfg.ConfigureEndpoints(ctx);
//             });
//         });
//     }
// }