
using System.Dynamic;

namespace App.Model;

public class AppDbContext: DbContext {

    public DbSet<Activity> Activities => Set<Activity>();

    public AppDbContext(DbContextOptions options): base(options) {

    }

    protected override  void OnModelCreating(ModelBuilder builder) {
        var activity = builder.Entity<Activity>();
        activity.ToTable("Activity");
        activity.HasKey("Id");
        activity.Property(t => t.Id).ValueGeneratedOnAdd();
        activity.HasIndex(t => new { t.Start, t.End });
        
    }
        

    
}