namespace App.Service;

public class BulkSaveActivitiesInput {
    public string OrganizationCode { get; set; } = "";
    public List<SaveActivityInput> Activities { get; set; } = new();
}