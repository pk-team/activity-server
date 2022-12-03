namespace App.Model; 

public class Entity {
    public Guid Id { get; set; }
    public DateTimeOffset Created { get; set; } = DateTime.UtcNow;
    public DateTimeOffset? Removed { get; set; }
}