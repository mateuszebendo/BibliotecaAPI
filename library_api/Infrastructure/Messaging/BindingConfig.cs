namespace library_api.Infrastructure.Messaging;

public class BindingConfig
{
    public string Exchange { get; set; }
    public string RoutingKey { get; set; }
}