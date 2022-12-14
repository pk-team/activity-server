
namespace App.Service.Validation;

public class BulkSaveActivityValidator : AbstractValidator<BulkSaveActivitiesInput> {
    public BulkSaveActivityValidator(AppDbContext context) {
        RuleFor(x => x.Activities).NotEmpty();
        RuleForEach(x => x.Activities.Select(t => t.ToActivity())).SetValidator(new ActivityValidator(context));
    }
}