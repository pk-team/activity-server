namespace App.Service;

public class SaveOrganizationInput {
    public Guid? Id { get; set; }
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string Address { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Contact { get; set; } = "";
    public string TaxId { get; set; } = "";
    public string HexColor { get; set; } = "";
}
