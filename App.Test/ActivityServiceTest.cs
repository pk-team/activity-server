
using App.Seed;

public class ActivityServiceTest : TestBase {

    [Fact]
    public async Task CanGetActivities() {
        // arange
        var dbContext = GetDbContext();
        var service = new ActivityService(dbContext);
        // act
        var result = await service.GetActivitiesAsync();
        // assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task Can_Add_Activity() {
        // arr
        var dbContext = GetDbContext();
        var organization = (await new SeedDataService(dbContext).CreateOrganizations(1)).First();
        var service = new ActivityService(dbContext);
        var input = new SaveActivityInput {
            Description = "Test",
            Start = DateTime.Now,
            End = DateTime.Now.AddHours(1),
            OrganizationId = organization.Id
        };
        // act
        var result = await service.SaveActivityAsync(input);
        // assert
        Assert.NotNull(result.SaveActivity);
        Assert.Equal(input.Description, result.SaveActivity.Description);
        Assert.Equal(input.Start, result.SaveActivity.Start);
        Assert.Equal(input.End, result.SaveActivity.End);
        Assert.Equal(input.OrganizationId, result.SaveActivity.OrganizationId);
    }

    [Fact]
    public async Task Can_Update_Activity() {
        // arrange
        var dbContext = GetDbContext();
        var organizations = await new SeedDataService(dbContext).CreateOrganizations(2);

        var service = new ActivityService(dbContext);
        var input = new SaveActivityInput {
            Description = "Test",
            Start = DateTime.Now,
            End = DateTime.Now.AddHours(1),
            OrganizationId = organizations.First().Id
        };
        var result = await service.SaveActivityAsync(input);
        var updateInput = new SaveActivityInput {
            Id = result.SaveActivity.Id,
            Description = "Test 2",
            Start = DateTime.Now,
            End = DateTime.Now.AddHours(2),
            OrganizationId = organizations.Last().Id
    };
        // act
        var updateResult = await service.SaveActivityAsync(updateInput);
        // assert
        Assert.NotNull(updateResult.SaveActivity);
        Assert.Equal(updateInput.Id, updateResult.SaveActivity.Id);
        Assert.Equal(updateInput.Description, updateResult.SaveActivity.Description);
        Assert.Equal(updateInput.Start, updateResult.SaveActivity.Start);
        Assert.Equal(updateInput.End, updateResult.SaveActivity.End);
        Assert.Equal(updateInput.OrganizationId, updateResult.SaveActivity.OrganizationId);
        // assert duration minutes is correct
        Assert.Equal((int)(updateInput.End - updateInput.Start).Value.TotalMinutes, updateResult.SaveActivity.DurationMinutes);
    }

    [Fact]
    public async Task Error_If_Activity_Start_End_Overlaps_With_Existing() {
        // arrange
        var dbContext = GetDbContext();
        var service = new ActivityService(dbContext);
        var input = new SaveActivityInput {
            Description = "Test",
            Start = DateTime.Now,
            End = DateTime.Now.AddHours(1)
        };
        var result = await service.SaveActivityAsync(input);
        var updateInput = new SaveActivityInput {
            Description = "Test 2",
            Start = DateTime.Now.AddMinutes(-30),
            End = DateTime.Now.AddHours(2)
        };
        // act
        var updateResult = await service.SaveActivityAsync(updateInput);
        // assert
        Assert.NotNull(updateResult.Errors);
        Assert.Single(updateResult.Errors);
        Assert.Equal("Activity overlaps with existing activity", updateResult.Errors[0].Message);

    }

    [Fact]
    public async Task Cannot_Update_Activity_If_ID_Not_Found() {
        // arrange
        var dbContext = GetDbContext();
        var service = new ActivityService(dbContext);
        var input = new SaveActivityInput {
            Id = Guid.NewGuid(),
            Description = "Test",
            Start = DateTime.Now,
            End = DateTime.Now.AddHours(1)
        };
        // act
        var result = await service.SaveActivityAsync(input);
        // assert
        Assert.NotNull(result.Errors);
        Assert.Single(result.Errors);
        Assert.Equal("Activity not found", result.Errors[0].Message);
    }

    public async Task Error_If_End_Before_Start() {
        // arrange
        var dbContext = GetDbContext();
        var service = new ActivityService(dbContext);
        var input = new SaveActivityInput {
            Description = "Test",
            Start = DateTime.Now,
            End = DateTime.Now.AddHours(-1)
        };
        // act
        var result = await service.SaveActivityAsync(input);
        // assert
        Assert.NotNull(result.Errors);
        Assert.Single(result.Errors);
        Assert.Equal("End date must be after start date", result.Errors[0].Message);
    }    

    [Fact]
    public async Task Description_Required() {
        // arrange
        var dbContext = GetDbContext();
        var service = new ActivityService(dbContext);
        var input = new SaveActivityInput {
            Start = DateTime.Now,
            End = DateTime.Now.AddHours(1)
        };
        // act
        var result = await service.SaveActivityAsync(input);
        // assert
        Assert.NotNull(result.Errors);
        Assert.Single(result.Errors);
        Assert.Equal("Description is required", result.Errors[0].Message);
    }
}