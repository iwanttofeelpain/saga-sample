namespace Host.Extension.Mvc.Settings;

public record RabbitSettings
{
    public string Uri { get; set; } = default!;

    public string VirtualHost { get; set; } = default!;

    public string Username { get; set; } = default!;

    public string Password { get; set; } = default!;

    public ushort Port { get; set; }
}
