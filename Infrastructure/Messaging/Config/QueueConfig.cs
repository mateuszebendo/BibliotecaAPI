namespace BibliotecaAPI.Infrastructure.Messaging;

public class QueueConfig
{
    public string Name { get; set; }
    public bool Durable { get; set; }
    public bool Exclusive { get; set; }
    public bool AutoDelete { get; set; }
    public Dictionary<string, object> Arguments { get; set; } = new Dictionary<string, object>();
    public List<BindingConfig> Bindings { get; set; } = new List<BindingConfig>();
}