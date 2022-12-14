namespace App.Service;

public class BulkSaveOrganizationsInput {
    public List<SaveOrganizationInput> Organizations { get; set; } = new List<SaveOrganizationInput>();
}