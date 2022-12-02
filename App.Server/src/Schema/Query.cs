namespace App.Server;

public class Query {
    public IQueryable<Activity> GetActivities(
        AppDbContext context
    ) => context.Activities;
}