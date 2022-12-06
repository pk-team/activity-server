using Microsoft.EntityFrameworkCore;

namespace App.Service;

public class ActivityService {
    private readonly AppDbContext _context;

    public ActivityService(AppDbContext context) {
        _context = context;
    }

    public async Task<Activity?> GetActivityAsync(Guid id) {
        return await _context.Activities.FindAsync(id);
    }

    public async Task<List<Activity>> GetActivitiesAsync() {
        return await _context.Activities.ToListAsync();
    }

    public async Task<AddActivityPayload> AddActivityAsync(AddActivityInput input) {
        var payload = new AddActivityPayload {
            Errors = new List<Error>()
        };

        var activity = new Activity{
            Description = input.Description,
            Start = input.Start,
            End = input.End
        };

        var validator = new ActivityValidation();
        var result = await validator.ValidateAsync(activity);

        if (!result.IsValid) {
            foreach (var error in result.Errors) {
                payload.Errors.Add(new Error {
                    Message = error.ErrorMessage
                });
            }
            return payload;
        }
        
        _context.Activities.Add(activity);
        await _context.SaveChangesAsync();

        payload.AddActivity = activity;
        return payload;
    }

    public async Task<UpdateActivityPayload> UpdateActivityAsync(UpdateActivityInput input) {
        var payload = new UpdateActivityPayload {
            Errors = new List<Error>()
        };

        var validator = new ActivityValidation();

        var activity = await _context.Activities.FirstOrDefaultAsync(t => t.Id == input.Id);
        if (activity == null) {
            payload.Errors.Add(new Error {
                Message = "Activity not found"
            });
            return payload;
        }

        var result = await validator.ValidateAsync(activity);
        payload.Errors = result.Errors.Select(t => new Error {
            Message = t.ErrorMessage
        }).ToList();

        if (payload.Errors.Any()) {
            return payload;
        }

        activity.Description = input.Description;
        activity.Start = input.Start;
        activity.End = input.End;

        await _context.SaveChangesAsync();

        payload.UpdateActivity = activity;
        return payload;
    }

    public async Task<Activity> DeleteActivityAsync(Guid id) {
        var activity = await _context.Activities.FirstAsync(t => t.Id == id);

        if (activity.RemovedAt == null) {
            activity.RemovedAt = DateTimeOffset.Now;
        }
        await _context.SaveChangesAsync();
        return activity;
    }
    public async Task<Activity> RestoreActivityAsync(Guid id) {
        var activity = await _context.Activities.FirstAsync(t => t.Id == id);

        if (activity.RemovedAt != null) {
            activity.RemovedAt = null;
        }
        await _context.SaveChangesAsync();
        return activity;
    }
}