using FluentValidation;

namespace App.Service;

public class ActivityValidation: AbstractValidator<Activity> {
    public ActivityValidation() {

        RuleFor(t => t.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(50).WithMessage("Description must be less than 50 characters");

        RuleFor(t => t.Start)
            .NotEmpty().WithMessage("Start date is required")
            .MustAsync(async (start, cancellation) => {
                return await Task.FromResult(start > DateTime.Now.AddYears(-1));
            }).WithMessage("Start date cannot be more than a year in the past");        

        RuleFor(t => new { t.Start, t.End }).Must(k => k.End != null && k.End > k.Start).WithMessage("End date must be greater than start date");
    }
}