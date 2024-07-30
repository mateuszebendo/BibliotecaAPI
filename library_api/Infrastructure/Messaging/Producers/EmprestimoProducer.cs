using System.Text;
using RabbitMQ.Client;

namespace library_api.Infrastructure.Messaging.Producers;

public class EmprestimoProducer
{
    private readonly IModel _channel;

    public EmprestimoProducer(IModel channel)
    {
        _channel = channel;
    }
}