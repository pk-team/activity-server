namespace App.Service;

public class ImportOrganizationPayload : MutationPayload {
    public List<Organization> ImportOrganizations { get; set; } = new List<Organization>();
}

