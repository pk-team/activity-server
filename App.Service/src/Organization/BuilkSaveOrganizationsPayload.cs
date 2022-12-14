namespace App.Service;

public class BulkSaveOrganizationsPayload : MutationPayload {
    public List<Organization> BulkSaveOrganizations { get; set; } = new List<Organization>();
}

