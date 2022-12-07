using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.Service.Validation;

public class ActivityValidator : AbstractValidator<Activity> {
    public ActivityValidator(AppDbContext context) {

        // activity not found 
        RuleFor(t => t.Id)
            .MustAsync(async (id, cx) => {
                if (id == Guid.Empty) {
                    return true;
                }
                var found =  await context.Activities.AnyAsync(t => t.Id == id);
                return found;
            }).WithMessage("Activity not found");

        // description required
        RuleFor(t => t.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(1000).WithMessage("Description must be less than 1000 characters");

        // start date required
        RuleFor(t => t.Start)
            .NotEmpty().WithMessage("Start date is required")
            .MustAsync(async (start, cx) => {
                await Task.Delay(10);
                return start > DateTime.Now.AddYears(-1);
            }).WithMessage("Start date cannot be more than a year in the past");

        RuleFor(t => new { t.Start, t.End }).Must(k => k.End != null && k.End > k.Start)
            .WithMessage("End date must be after start date");

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