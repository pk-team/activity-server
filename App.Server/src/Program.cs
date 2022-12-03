using App.Servier;

var builder = WebApplication.CreateBuilder();

var appSetting = new AppSetting(builder.Configuration);

// services
builder.Services.AddPooledDbContextFactory<AppDbContext>(
        options => options.UseSqlServer(appSetting.Connectionstring, sqlOptions => {
            sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        }))
    .AddScoped<AppDbContext>(p => p.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext())
    .AddScoped<SeedDataService>()
    .AddScoped<CustomQueryService>();

// graphql server
builder.Services.AddGraphQLServer()
    .RegisterDbContext<AppDbContext>(DbContextKind.Pooled)
    .AddQueryType<Query>()
        .AddTypeExtension<CustomQuery>()
    .InitializeOnStartup();

// build
var app = builder.Build();

// endpoints
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

// run
app.Run();


