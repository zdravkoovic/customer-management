using System.Text.Json;
using Confluent.Kafka;
using Customer.Application.Abstractions.Messaging;
using Customer.Core.src.Events;

namespace Customer.Infrastructure.src.Messaging.Kafka;

public class KafkaIntegrationEventPublisher : IIntegrationEventPublisher
{
    private readonly IProducer<string, string> _producer;
    private readonly string _defaultTopic;

    public KafkaIntegrationEventPublisher(string bootstrapServers, string topic)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = bootstrapServers,
            Acks = Acks.All, // Ensure all replicas acknowledge
            EnableIdempotence = true, // Ensure exactly-once delivery
            CompressionType = CompressionType.Snappy, // Optimize message size
            LingerMs = 5 // Reduce latency, Batch messages for better throughput
        };

        _producer = new ProducerBuilder<string, string>(config).Build();
        _defaultTopic = topic;
    }

    public async Task PublishAsync<TEvent>(TEvent @event,CancellationToken cancellationToken = default) where TEvent : IntegrationEvent
    {
        var topic = _defaultTopic ?? @event.GetType().Name;

        var message = JsonSerializer.Serialize(@event);

        try
        {
            var kafkaMessage = new Message<string, string>
            {
                Key = @event.AggregateId.ToString(),
                Value = message
            };

            var deliveryResult = await _producer.ProduceAsync(topic, kafkaMessage);

            Console.WriteLine($"Delivered event to Kafka: {deliveryResult.TopicPartitionOffset}");
        }
        catch (ProduceException<string, string> ex)
        {
            Console.WriteLine($"Failed to deliver event to Kafka: {ex.Error.Reason}");
            throw;
        }
    }

    public async Task PublishAsync<T>(T @event) where T : IntegrationEvent
    {
        // Determine the topic based on the event type
            var topic = _defaultTopic ?? @event.GetType().Name;

            // Serialize the event to JSON
            var message = JsonSerializer.Serialize(@event);

            try
            {
                // Produce the message
                var kafkaMessage = new Message<string, string>
                {
                    Key = @event.AggregateId.ToString(),
                    Value = message
                };

                var deliveryResult = await _producer.ProduceAsync(topic, kafkaMessage);

                Console.WriteLine($"Delivered event to Kafka: {deliveryResult.TopicPartitionOffset}");
            }
            catch (ProduceException<string, string> ex)
            {
                // Log the exception or handle it as per your needs
                Console.WriteLine($"Failed to deliver event: {ex.Message}");
                throw;
            }
    }
}