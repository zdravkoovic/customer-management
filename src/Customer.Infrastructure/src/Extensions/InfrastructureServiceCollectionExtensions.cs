using Confluent.Kafka;
using Customer.Application.Abstractions.Messaging;
using Customer.Application.Customer.Repositories;
using Customer.Core.src.Interfaces;
using Customer.Infrastructure.Persistance.Repositories;
using Customer.Infrastructure.src.Email;
using Customer.Infrastructure.src.Messaging.Kafka;
using Customer.Infrastructure.src.Persistance;
using Customer.Infrastructure.src.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Customer.Infrastructure.src.Extensions;

public static class InfrastructureServiceExtensions
{
    public static void ApplyMigrations(this IHost app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<AppDbContext>();
            context.Database.Migrate();
            Console.WriteLine("Database migrations applied successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while applying migrations: {ex.Message}");
            throw;
        }
    }
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IIntegrationEventLog, IntegrationEventLog>();
        
        services.AddAutoMapper(cfg => {}, typeof(InfrastructureServiceExtensions).Assembly);

        var kafkaConfig = configuration.GetSection("Kafka");
        var topics = kafkaConfig.GetSection("Topics")
            .Get<Dictionary<string, string>>() ?? new Dictionary<string, string>();
        
        services.AddSingleton(topics);

        services.AddSingleton(sp =>
        {
           var config = new ConsumerConfig
            {
                BootstrapServers = kafkaConfig["BootstrapServers"],
                GroupId = kafkaConfig["GroupId"],
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false  
            };

           return new ConsumerBuilder<string, string>(config).Build();
        });

        services.AddHostedService<KafkaIntegrationEventConsumer>();

        services.AddSingleton(sp =>
        {
            var config = new ProducerConfig
            {
                BootstrapServers = kafkaConfig["BootstrapServers"],
                Acks = Acks.All,
                EnableIdempotence = true,
                MessageSendMaxRetries = 5,
                LingerMs = 5,
                BatchSize = 32 * 1024  
            };

            return new ProducerBuilder<string, string>(config).Build();
        });

        services.AddHostedService<KafkaIntegrationEventPublisher>();


        services.AddScoped<IEmailSender, MimeKitEmailSender>();
        
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        return services;
    }
}