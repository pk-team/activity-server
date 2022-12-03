using App.Seed;

var builder = WebApplication.CreateBuilder();


var appSetting = new AppSetting(builder.Configuration);

Console.WriteLine(appSetting.Connectionstring);
Console.WriteLine(appSetting.AppTitle);

builder.Services.AddPooledDbContextFactory<AppDbContext>(
        options => options.UseSqlServer(appSetting.Connectionstring, sqlOptions => {
            sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        }))
    .AddScoped<AppDbContext>(p => p.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext())
    .AddScoped<SeedDataService>();

builder.Services.AddGraphQLServer()
    .RegisterDbContext<AppDbContext>(DbContextKind.Pooled)
    .AddQueryType<Query>();


var app = builder.Build();

app.MapGet("/", () => "Timesheet graphql server");




app.MapPost("/ensure-db", async (AppDbContext context) => {
    if (app.Environment.IsDevelopment()) {
        await context.Database.EnsureCreatedAsync();
    }
});

app.MapPost("/ensure-migration", async (AppDbContext context) => {
    await context.Database.MigrateAsync();
});

app.MapPost("/seed-db", async (AppDbContext context, [Service] SeedDataService service) => {
    if (app.Environment.IsDevelopment()) {
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
        await service.SeedDatabase();
    }
});

app.MapGraphQL();

app.Run();


