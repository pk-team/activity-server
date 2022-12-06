
using App.Model;
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
        return dbContext;
        
    }

}