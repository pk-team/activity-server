namespace App.Service;

public class AddActivityInput {
    public string Description { get; set; } = "";
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset? End { get; set; }
}