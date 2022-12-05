namespace App.Service;

public class AddActivityPayload : MutationPayload {
    public Activity AddActivity { get; set; } = new Activity();
}