namespace App.Service;

public class ImportOrganizationsInput {
    public List<SaveOrganizationInput> Organizations { get; set; } = new List<SaveOrganizationInput>();
}