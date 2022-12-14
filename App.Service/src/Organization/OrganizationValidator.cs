using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.Service.Validation;

public class OrganizationValidator : AbstractValidator<Organization> {

    
    public OrganizationValidator(AppDbContext context) {
        RuleFor(t => t.Name).NotEmpty().WithMessage("Organization name is required")
            .MaximumLength(150).WithMessage("Organization name must be less than 150 characters");

        RuleFor(t => t.Code).NotEmpty()
            .WithMessage("Organization code is required")
            .MaximumLength(11).WithMessage("Organization code must be less than or equal to 10 characters");
        
        RuleFor(t => new { t.Id, t.Code})
            .MustAsync(async (k, cx) => {
                var found = await context.Organizations.AnyAsync(t => t.Code == k.Code && t.Id != k.Id);
                return !found;
            }).WithMessage("Organization code already in use by another organization");

        RuleFor(t => new { t.Id, t.Name})
            .MustAsync(async (k, cx) => {
                var found = await context.Organizations.AnyAsync(t => t.Name == k.Name && t.Id != k.Id);
                return !found;
            }).WithMessage("Organization name already in use by another organization");

        RuleFor(t => t.TaxId)
            .MaximumLength(50).WithMessage("Tax ID must be less than 50 characters");

        RuleFor(t => new { t.Id, t.TaxId })
            .MustAsync(async (k, cx) => {
                var found = await context.Organizations.AnyAsync(t => t.TaxId == k.TaxId && t.Id != k.Id);
                return !found;
            }).WithMessage("Tax ID already in use by another organization");


    }
}