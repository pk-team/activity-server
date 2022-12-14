
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
        };
        var result = await service.SaveOrganizationAsync(input);
        var updateInput = new SaveOrganizationInput {
            Id = result.SaveOrganization.Id,
            Code = "Code 2",
            Name = "Name 2",
            Phone = "12344994 2",
            Address = "1234 Thailand 2",
            Contact = "Dude 2",
        };
        // act
        var updateResult = await service.SaveOrganizationAsync(updateInput);
        // assert
        Assert.NotNull(updateResult.SaveOrganization);
        Assert.Equal(updateInput.Code, updateResult.SaveOrganization.Code);
        Assert.Equal(updateInput.Name, updateResult.SaveOrganization.Name);
        Assert.Equal(updateInput.Phone, updateResult.SaveOrganization.Phone);
        Assert.Equal(updateInput.Address, updateResult.SaveOrganization.Address);
        Assert.Equal(updateInput.Contact, updateResult.SaveOrganization.Contact);
        Assert.Equal(updateInput.TaxId, updateResult.SaveOrganization.TaxId);
    }
}
