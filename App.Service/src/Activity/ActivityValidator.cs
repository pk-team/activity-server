using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.Service.Validation;

public class ActivityValidator {

    AppDbContext _context;
    public ActivityValidator(AppDbContext context) {
        this._context = context;
    }

    public async Task<List<Error>> ValidateAsync(Activity activity) {
        var errors = new List<Error>();

        if (activity.Id != Guid.Empty) {
            var existing = await _context.Activities.FindAsync(activity.Id);
            if (existing == null) {
                errors.Add(new Error {
                    Message = "Activity not found",
                    Path = new List<string> { "Id" }
                });
            }
        }

        var propValidator = await new ActivityPropertyValidator(_context).ValidateAsync(activity);
        if (!propValidator.IsValid) {
            errors.AddRange(propValidator.Errors.Select(t => new Error {
                Message = t.ErrorMessage,
                Path = new List<string> { t.PropertyName }
            }));
        }
        // }

        // overlapping activities
        // var overlapping = await _context.Activities
        //     .Where(t => t.Id != activity.Id)
        //     .Where(t => t.RemovedAt == null)
        //     .Where(t => t.Start < activity.End)
        //     .Where(t => t.End > activity.Start)
        //     .ToListAsync();

        // if (overlapping.Any()) {
        //     errors.Add(new Error {
        //         Message = "Activity overlaps with existing activity",
        //         Path = new List<string> { "Start", "End" }
        //     });
        // }
        return errors;
    }
}

public class ActivityPropertyValidator : AbstractValidator<Activity> {
    public ActivityPropertyValidator(AppDbContext context) {

        RuleFor(t => t.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(50).WithMessage("Description must be less than 50 characters");

        RuleFor(t => t.Start)
            .NotEmpty().WithMessage("Start date is required")
            .MustAsync(async (start, cx) => {
                await Task.Delay(10);
                return start > DateTime.Now.AddYears(-1);
            }).WithMessage("Start date cannot be more than a year in the past");

        RuleFor(t => new { t.Start, t.End }).Must(k => k.End != null && k.End > k.Start)
            .WithMessage("End date must be greater than start date");

        // overlapping start end    
        RuleFor(t => new { t.Id, t.Start, t.End }).MustAsync(async (k, cx) => {
            var overlapping = await context
                .Activities
                .Where(t => t.Id != k.Id)
                .Where(t => t.RemovedAt == null)
                .Where(t => t.Start < k.End)
                .Where(t => t.End > k.Start)
                .ToListAsync();
            return !overlapping.Any();
        }).WithMessage("Activity overlaps with existing activity");
        


    }
}