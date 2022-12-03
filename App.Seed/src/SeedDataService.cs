using System.Data;

namespace App.Seed;

public class SeedDataService {
    AppDbContext context;
    public SeedDataService(AppDbContext context) {
        this.context = context;
    }

    public async Task SeedDatabase() {
        // activities
        await CreateActivities(new CreateActivitiesOption());
    }

    public record CreateActivitiesOption(int durationMinutes = 60, int activitiesPerDay = 2, int days = 5);
    public async Task CreateActivities(CreateActivitiesOption option) {

        TimeOnly startTime = new(10, 0);
        DateTimeOffset startDate = DateTime.Now.AddDays(-option.days).Date;

        var dayIndex = 0;
        int count = 0;
        DateTimeOffset currentDate = startDate;
        while (dayIndex < option.days) {
            var activityIndex = 0;
            var startDateTime = currentDate.AddTicks(startTime.Ticks);
            while (activityIndex < option.activitiesPerDay) {
                
                var activity = new Activity {
                    Description = $"Activity description {++count}",
                    Start = startDateTime,
                    End = startDateTime.AddMinutes(option.durationMinutes),
                    DurationMinutes = option.durationMinutes                    
                };
                context.Activities.Add(activity);

                startDateTime = startDateTime.AddMinutes(option.durationMinutes);
                activityIndex++;
            }

            currentDate = currentDate.AddDays(1);
            dayIndex++;
        }

        await context.SaveChangesAsync();
    }
}