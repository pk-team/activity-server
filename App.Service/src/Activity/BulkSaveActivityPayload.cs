namespace App.Service;

public class BulkSaveActivitiesPayload : MutationPayload {
    public List<Activity> BulkSaveActivities { get; set; } = new();
}

