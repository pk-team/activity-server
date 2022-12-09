namespace App.Server;

[ExtendObjectType(typeof(Query))]
public class ProjectionQuery {

    [UsePaging(MaxPageSize = 1000)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Activity> GetActivities(
        [ScopedService] AppDbContext context
    ) => context.Activities;

    // get Organiztions
    [UsePaging(MaxPageSize = 1000)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Organization> GetOrganizations(
        [ScopedService] AppDbContext context
    ) => context.Organizations;

}