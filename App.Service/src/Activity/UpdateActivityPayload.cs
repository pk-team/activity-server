namespace App.Service;

public class UpdateActivityPayload : MutationPayload {
    public Activity UpdateActivity { get; set; } = new Activity();
}
