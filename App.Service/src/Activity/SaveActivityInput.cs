namespace App.Service;

public class SaveActivityInput {
    public Guid? Id { get; set; }
    public string Description { get; set; } = "";
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset? End { get; set; }
    public Guid? OrganizationId { get; set; }

    // To Activity
    public Activity ToActivity() {
        return new Activity {
            Id = Id ?? Guid.NewGuid(),
            Description = Description,
            Start = Start,
            End = End,
            OrganizationId = OrganizationId
        };
    }

}