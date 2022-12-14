namespace App.Service;

using App.Service.Validation;
using Microsoft.EntityFrameworkCore;

public class OrganizationService {
    private AppDbContext _context;

    public OrganizationService(AppDbContext context) {
        this._context = context;
    }

    // save organization
    public async Task<SaveOrganizationPayload> SaveOrganizationAsync(SaveOrganizationInput input) {
        var organization = await _context.Organizations.FindAsync(input.Id);
        if (organization == null) {
            organization = new Organization();
        }

        organization.Id = input.Id ?? Guid.Empty; // must set to empty 
        organization.Code = input.Code;
        organization.Name = input.Name;
        organization.Address = input.Address;
        organization.Phone = input.Phone;
        organization.Contact = input.Contact;
        organization.TaxId = input.TaxId;        

        var validator = new OrganizationValidator(_context);
        var validationResult = await validator.ValidateAsync(organization);
        var payload = new SaveOrganizationPayload {
            Errors = validationResult.Errors.Select(t => new Error {
                Message = t.ErrorMessage,
                Path = t.PropertyName.Split('.').ToList()
            }).ToList()
        };

        if (payload.Errors.Any()) {
            return payload;
        }

        if (input.Id.HasValue && input.Id.Value != Guid.Empty) {
            _context.Organizations.Update(organization);
        } else {
            _context.Organizations.Add(organization);
        }
        await _context.SaveChangesAsync();

        payload.SaveOrganization = organization;
        return payload;
    }
}