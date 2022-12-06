namespace App.Model; 

public class Entity {
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTimeOffset? RemovedAt { get; set; }
}