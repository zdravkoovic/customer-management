using System.Text.Json;

namespace Customer.Infrastructure.src.Messaging.Kafka;

public static class KafkaJsonSerializerOptions
{
    public static readonly JsonSerializerOptions Default = new()
    {
        PropertyNameCaseInsensitive = true
    };
}