
var builder = WebApplication.CreateBuilder();


var appSetting = new AppSetting(builder.Configuration);

Console.WriteLine(appSetting.Connectionstring);
Console.WriteLine(appSetting.AppTitle);

builder.Services.AddPooledDbContextFactory<AppDbContext>(
        options => options.UseSqlServer(appSetting.Connectionstring, sqlOptions => {
            sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        }))
    .AddScoped<AppDbContext>(p => p.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext());

builder.Services.AddGraphQLServer()
    .RegisterDbContext<AppDbContext>(DbContextKind.Pooled)
    .AddQueryType<Query>();


var app = builder.Build();

app.MapGet("/", () => "Timesheet graphql server");

app.MapPost("/ensure", async (AppDbContext context) => {
    await context.Database.EnsureDeletedAsync();
    await context.Database.EnsureCreatedAsync();
    context.Activities.Add(new Activity {
        Description = "Setup Timesheet Grraphql Server",
        Start = new DateTimeOffset(DateTime.Now)
    });
    await context.SaveChangesAsync();
});

app.MapGraphQL();

app.Run();


