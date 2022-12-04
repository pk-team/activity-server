using Microsoft.Identity.Client;

namespace App.Model;

public enum OrganizationType  {
    CUSTOMER,
    SELF
}

public class Organization : Entity {    
    public OrganizationType OrganizationType { get; set; } 
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string NameLocal { get; set; } = "";
    public string Address { get; set; } = "";
    public string AddressLocal { get; set; } = "";
    public string Phone { get; set; } = "";
    public string TaxID { get; set; } = "";
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset RemovedAt { get; set; }

    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    public ICollection<Activity> Activities { get; set; } = new List<Activity>();
}