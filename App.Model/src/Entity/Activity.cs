namespace App.Model;

public class Activity : Entity {
    public string Description { get; set; } = "";
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset? End { get; set; }
    public int DurationMinutes { get; set; }

    public Invoice? Invoice { get; set;  }
    public Organization? Customer { get; set;  }
}