namespace App.Service;

public class SaveOrganizationPayload : MutationPayload {
    public Organization SaveOrganization { get; set; } = new Organization();
}