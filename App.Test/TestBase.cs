
using App.Model;
using App.Seed;
using Microsoft.Data.Sqlite;

public class TestBase {

    public AppDbContext GetDbContext() {        

        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        var dbContext = new AppDbContext(options);
        dbContext.Database.EnsureCreated();

        var seedData = new SeedDataService(dbContext);
        seedData.ClearData();

        return dbContext;
        
    }

}