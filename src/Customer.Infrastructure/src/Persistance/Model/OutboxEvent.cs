using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Customer.Infrastructure.src.Persistance.Model;

[Index(nameof(OccurredAt), nameof(ProcessedAt), nameof(AggregateId), nameof(LockedUntil))]
public class OutboxEvent
{
    [Key]
    public Guid Id { get; set; }
    public string Type { get; set; } = null!;
    public string Payload { get; set; } = null!;
    public Guid? AggregateId { get; set; }
    public DateTime OccurredAt { get; set; }
    public DateTime? ProcessedAt { get; set; }

    public int AttemptCount { get; set; }
    public string? Error { get; set; }

    public DateTime? LockedUntil { get; set; }
}