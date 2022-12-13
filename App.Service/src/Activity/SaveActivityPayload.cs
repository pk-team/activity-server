namespace App.Service;

public class SaveActivityPayload : MutationPayload {
    public Activity SaveActivity { get; set; } = new Activity();
}