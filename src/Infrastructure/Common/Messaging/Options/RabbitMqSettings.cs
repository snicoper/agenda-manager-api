using System.ComponentModel.DataAnnotations;

namespace AgendaManager.Infrastructure.Common.Messaging.Options;

public class RabbitMqSettings
{
    public const string SectionName = "RabbitMq";

    [Required]
    public string Host { get; set; } = null!;

    [Required]
    public string User { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public int Port { get; set; }

    [Required]
    public string Exchange { get; set; } = null!;

    [Required]
    public string QueueName { get; set; } = null!;
}
