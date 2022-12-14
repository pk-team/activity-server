using App.Service.Validation;
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

    public async Task<SaveActivityPayload> SaveActivityAsync(SaveActivityInput input) {
        var activity = await _context.Activities.FindAsync(input.Id);
        if (activity == null) {
            activity = new Activity();
        }

        activity.Id = input.Id ?? Guid.Empty; // must set to empty 
        activity.Description = input.Description;
        activity.Start = input.Start;
        activity.End = input.End;
        activity.OrganizationId = input.OrganizationId;
        // duration minutes if end is set
        activity.DurationMinutes = input.End != null ? (int)(input.End - input.Start).Value.TotalMinutes : 0;

        var validator = new ActivityValidator(_context);
        var validationResult = await validator.ValidateAsync(activity);
        var payload = new SaveActivityPayload {
            Errors = validationResult.Errors.Select(t => new Error {
                Message = t.ErrorMessage,
                Path = t.PropertyName.Split('.').ToList()
            }).ToList()
        };

        if (payload.Errors.Any()) {
            return payload;
        }

        if (input.Id.HasValue && input.Id.Value != Guid.Empty) {
            _context.Activities.Update(activity);
        } else {
            _context.Activities.Add(activity);
        }
        await _context.SaveChangesAsync();

        payload.SaveActivity = activity;
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

    // bulk save activities
    public async Task<BulkSaveActivitiesPayload> BulkSaveActivitiesAsync(BulkSaveActivitiesInput input) {
        using var transaction = _context.Database.BeginTransaction();

        var payload = new BulkSaveActivitiesPayload();

        var organization = await _context.Organizations.FirstOrDefaultAsync(t => t.Code == input.OrganizationCode);

        foreach (var item in input.Activities) {
            item.OrganizationId = organization?.Id;

            var existing = await _context.Activities.FirstOrDefaultAsync(t => t.OrganizationId == item.OrganizationId && t.Start == item.Start && t.End == item.End);
            if (existing == null) {
                var savePayload = await SaveActivityAsync(item);
                payload.Errors.AddRange(savePayload.Errors);
                // break if errors
                // if (payload.Errors.Any()) {
                //     break;
                // }
                payload.BulkSaveActivities.Add(savePayload.SaveActivity);
            }
        }

        if (payload.Errors.Any()) {
            await transaction.RollbackAsync();
            return payload;
        }

        await transaction.CommitAsync();
        return payload;
    }
}