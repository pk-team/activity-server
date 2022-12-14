
using App.Seed;

public class OrganizationServiceTest : TestBase {

    [Fact]
    public async Task Can_Add_Organization() {
        // arrange
        var dbContext = GetDbContext();
        var service = new OrganizationService(dbContext);
        var input = new SaveOrganizationInput {
            Code = "Code",
            Name = "Name",
            Phone = "12344994",
            Address = "1234 Thailand",
            Contact = "Dude",
            HexColor = "#000000"
        };
        // act
        var result = await service.SaveOrganizationAsync(input);
        // assert
        Assert.NotNull(result.SaveOrganization);
        Assert.Equal(input.Code, result.SaveOrganization.Code);
        Assert.Equal(input.Name, result.SaveOrganization.Name);
        Assert.Equal(input.Phone, result.SaveOrganization.Phone);
        Assert.Equal(input.Address, result.SaveOrganization.Address);
        Assert.Equal(input.Contact, result.SaveOrganization.Contact);
        Assert.Equal(input.TaxId, result.SaveOrganization.TaxId);
        Assert.Equal(input.HexColor, result.SaveOrganization.HexColor);
    }

    [Fact]
    public async Task Can_Update_Organization() {
        // arrange
        var dbContext = GetDbContext();
        var service = new OrganizationService(dbContext);
        var input = new SaveOrganizationInput {
            Code = "Code",
            Name = "Name",
            Phone = "12344994",
            Address = "1234 Thailand",
            Contact = "Dude",
            HexColor = "#000000"
        };
        var result = await service.SaveOrganizationAsync(input);
        var updateInput = new SaveOrganizationInput {
            Id = result.SaveOrganization.Id,
            Code = "Code 2",
            Name = "Name 2",
            Phone = "12344994 2",
            Address = "1234 Thailand 2",
            Contact = "Dude 2",
            HexColor = "#aaaaaa"
        };
        // act
        dbContext.ChangeTracker.Clear();
        var updateResult = await service.SaveOrganizationAsync(updateInput);
        // assert
        Assert.NotNull(updateResult.SaveOrganization);
        Assert.Equal(updateInput.Code, updateResult.SaveOrganization.Code);
        Assert.Equal(updateInput.Name, updateResult.SaveOrganization.Name);
        Assert.Equal(updateInput.Phone, updateResult.SaveOrganization.Phone);
        Assert.Equal(updateInput.Address, updateResult.SaveOrganization.Address);
        Assert.Equal(updateInput.Contact, updateResult.SaveOrganization.Contact);
        Assert.Equal(updateInput.TaxId, updateResult.SaveOrganization.TaxId);
        Assert.Equal(updateInput.HexColor, updateResult.SaveOrganization.HexColor);
    }

    [Fact]
    public async Task Can_Bulk_Save_Organizations() {
        // arrange
        var dbContext = GetDbContext();
        var service = new OrganizationService(dbContext);
        var input = new BulkSaveOrganizationsInput {
            Organizations = new List<SaveOrganizationInput> {
                new SaveOrganizationInput {
                    Code = "Code",
                    Name = "Name",
                    Phone = "12344994",
                    Address = "1234 Thailand",
                    Contact = "Dude",
                    TaxId = "1234567890123",
                    HexColor = "#000000"
                },
                new SaveOrganizationInput {
                    Code = "Code 2",
                    Name = "Name 2",
                    Phone = "12344994 2",
                    Address = "1234 Thailand 2",
                    Contact = "Dude 2",
                    TaxId = "1234567890123 2",
                    HexColor = "#aaaaaa"
                }
            }
        };
        var result = await service.BulkSaveOrganizationsAsync(input);
        // assert
        dbContext.ChangeTracker.Clear();

        Assert.NotNull(result.BulkSaveOrganizations);
        Assert.Equal(input.Organizations.Count, result.BulkSaveOrganizations.Count);
        Assert.Empty(result.Errors);

        foreach(var inputOrganization in input.Organizations) {
            var organization = await dbContext.Organizations.FirstOrDefaultAsync(x => x.Code == inputOrganization.Code);
            Assert.NotNull(organization);
            // name
            Assert.Equal(inputOrganization.Name, organization.Name);
            Assert.Equal(inputOrganization.Phone, organization.Phone);
            Assert.Equal(inputOrganization.Address, organization.Address);
            Assert.Equal(inputOrganization.Contact, organization.Contact);
            Assert.Equal(inputOrganization.TaxId, organization.TaxId);
            Assert.Equal(inputOrganization.HexColor, organization.HexColor);
        }

    }

}

