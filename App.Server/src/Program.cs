using App.Servier;

var builder = WebApplication.CreateBuilder();

var appSetting = new ServerSetting(builder.Configuration);

// services
builder.Services.AddPooledDbContextFactory<AppDbContext>(
        options => options.UseSqlServer(appSetting.Connectionstring, sqlOptions => {
            sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        }))
    .AddScoped<AppDbContext>(p => p.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext())
    .AddScoped<SeedDataService>()
    .AddScoped<CustomQueryService>()
    .AddScoped<ActivityService>()
    .AddScoped<OrganizationService>();

// graphql server
builder.Services.AddGraphQLServer()
    .RegisterDbContext<AppDbContext>(DbContextKind.Pooled)
    .AddQueryType<Query>()
        .AddTypeExtension<CustomQuery>()
        .AddTypeExtension<ProjectionQuery>()
    .AddMutationType<Mutation>()
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    .InitializeOnStartup();

// build
var app = builder.Build();

// endpoints

#region minimal api endpoints
app.MapGet("/", () => "Timesheet graphql server");
app.MapPost("/create-db", async (AppDbContext context) => {
    if (app.Environment.IsDevelopment()) {
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }
});

app.MapPost("/migrate-db", async (AppDbContext context) => {
    await context.Database.MigrateAsync();
});

app.MapPost("/clear-db", (AppDbContext context, [Service] SeedDataService service) => {
    if (app.Environment.IsDevelopment()) {
         service.ClearData();
    }
});

app.MapPost("/seed-db", (AppDbContext context, [Service] SeedDataService service) => {
    if (app.Environment.IsDevelopment()) {
         service.ClearAndReseedDatabase();
    }
});

#endregion

app.MapGraphQL();

// run
app.Run();


