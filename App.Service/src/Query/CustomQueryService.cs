using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;

namespace App.Service;
public class CustomQueryService {
    AppDbContext context;
    public CustomQueryService(AppDbContext context) {
        this.context = context;
    }

    public async Task<List<ActivityDTO>> GetOverlappingActivities() {
        var result = await (
            from a in context.Activities where a.RemovedAt == null
            from b in context.Activities where b.RemovedAt == null
            where a.Id != b.Id && a.Start < b.End && a.End > b.Start
            select new ActivityDTO {
                Id = a.Id,
                Description = a.Description,
                Start = a.Start,
                End = a.End,
                DurationMinutes = a.DurationMinutes,
                CreatedAt = a.CreatedAt,
                RemovedAt = a.RemovedAt
            }
        ).ToListAsync();

        return  result;
    }

}   

