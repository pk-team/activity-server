
using System.Dynamic;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace App.Model;

public class AppDbContext : DbContext {

    public DbSet<Organization> Customes => Set<Organization>();
    public DbSet<Activity> Activities => Set<Activity>();

    public AppDbContext(DbContextOptions options) : base(options) {

    }

    protected override void OnModelCreating(ModelBuilder builder) {

        if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite") {
            // SQLite does not have proper support for DateTimeOffset via Entity Framework Core, see the limitations
            // here: https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations
            // To work around this, when the Sqlite database provider is used, all model properties of type DateTimeOffset
            // use the DateTimeOffsetToBinaryConverter
            // Based on: https://github.com/aspnet/EntityFrameworkCore/issues/10784#issuecomment-415769754
            // This only supports millisecond precision, but should be sufficient for most use cases.
            foreach (var entityType in builder.Model.GetEntityTypes()) {
                var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(DateTimeOffset)
                                                                            || p.PropertyType == typeof(DateTimeOffset?));
                foreach (var property in properties) {
                    builder
                        .Entity(entityType.Name)
                        .Property(property.Name)
                        .HasConversion(new DateTimeOffsetToBinaryConverter());
                }
            }
        }


        var activity = builder.Entity<Activity>();
        activity.ToTable("Activity");
        activity.HasKey("Id");
        activity.Property(t => t.Id).ValueGeneratedOnAdd();
        activity.HasIndex(t => new {
            t.Start, t.End
        });

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