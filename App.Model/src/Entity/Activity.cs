namespace App.Model;

public class Activity : BaseEntity {
    public string Description { get; set; } = "";
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset? End { get; set; }
    public int DurationMinutes { get; set; }
}