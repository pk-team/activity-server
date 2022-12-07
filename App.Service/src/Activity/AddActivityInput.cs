namespace App.Service;

public class SaveActivityInput {
    public Guid? Id { get; set; }
    public string Description { get; set; } = "";
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset? End { get; set; }

    // to activity
}