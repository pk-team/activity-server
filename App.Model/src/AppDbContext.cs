
using System.Dynamic;

namespace App.Model;

public class AppDbContext: DbContext {

    public DbSet<Organization> Customes => Set<Organization>();
    public DbSet<Activity> Activities => Set<Activity>();

    public AppDbContext(DbContextOptions options): base(options) {

    }

    protected override  void OnModelCreating(ModelBuilder builder) {

        var activity = builder.Entity<Activity>();
        activity.ToTable("Activity");
        activity.HasKey("Id");
        activity.Property(t => t.Id).ValueGeneratedOnAdd();
        activity.HasIndex(t => new { t.Start, t.End });

        var organization = builder.Entity<Organization>();
        organization.ToTable("Organization");
        organization.HasKey("Id");
        organization.Property(t => t.Id).ValueGeneratedOnAdd();
        organization.HasIndex(t => t.Code).IsUnique();
        organization.HasIndex(t => t.Name).IsUnique();

        var invoice = builder.Entity<Invoice>();
        invoice.ToTable("Invoice");
        invoice.HasKey("Id");
        invoice.Property(t => t.Id).ValueGeneratedOnAdd();
        invoice.HasIndex(t => new { t.Year, t.Number }).IsUnique();

    }
}