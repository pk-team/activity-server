namespace App.Seed;

public class SeedDataService {
    AppDbContext context;
    public SeedDataService(AppDbContext context) {
        this.context = context;
    }

    public void ClearAndReseedDatabase() {
         ClearData();
         SeedData();
    }

    public void ClearData() {
         context.Database.ExecuteSqlRaw("DELETE FROM Invoice");
         context.Database.ExecuteSqlRaw("DELETE FROM Activity");
         context.Database.ExecuteSqlRaw("DELETE FROM Organization");
    }

    public void SeedData() {
        // activities
        var organizations = ( CreateOrganizations(1)).First();
    }

    public List<Organization> CreateOrganizations(int count = 10) {
        var organizations = new List<Organization>();
        for (int i = 0; i < count; i++) {
            var organization = new Organization {
                Name = $"Organization {i + 1}",
                Code = $"Code{i + 1}",
                Address = $"12{i + 1} Main St",
                Contact = $"John Doe {i + 1}",
                Phone = $"555-555-{i + 1}{i + 1}{i + 1}{i + 1}",
            };
            organizations.Add(organization);
            context.Organizations.Add(organization);
        }

         context.SaveChangesAsync();
        return organizations;
    }


}