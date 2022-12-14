namespace App.Model;

public class Organization : Entity {    
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string Address { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Contact { get; set; } = "";
    public string TaxId { get; set; } = "";
    public string? HexColor { get; set;  } = "";
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    public ICollection<Activity> Activities { get; set; } = new List<Activity>();
}