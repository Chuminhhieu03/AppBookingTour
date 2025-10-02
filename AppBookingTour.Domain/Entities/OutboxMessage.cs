namespace AppBookingTour.Domain.Entities;

public class OutboxMessage : BaseEntity
{
    public string Type { get; set; } = null!;
    public string Payload { get; set; } = null!;
    public DateTime? ProcessedAt { get; set; }
    public int AttemptCount { get; set; } = 0;
    public string? Error { get; set; }
}