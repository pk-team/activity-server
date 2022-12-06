
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
    public async Task CanAddActivity() {
        // arr
        var dbContext = GetDbContext();
        var service = new ActivityService(dbContext);
        var input = new AddActivityInput {
            Description = "Test",
            Start = DateTime.Now,
            End = DateTime.Now.AddHours(1)
        };
        // act
        var result = await service.AddActivityAsync(input);
        // assert
        Assert.NotNull(result.AddActivity);
        Assert.Equal(input.Description, result.AddActivity.Description);
        Assert.Equal(input.Start, result.AddActivity.Start);
        Assert.Equal(input.End, result.AddActivity.End);
    }

    [Fact]
    public async Task CanUpdateActivity() {
        // arrange
        var dbContext = GetDbContext();
        var service = new ActivityService(dbContext);
        var input = new AddActivityInput {
            Description = "Test",
            Start = DateTime.Now,
            End = DateTime.Now.AddHours(1)
        };
        var result = await service.AddActivityAsync(input);
        var updateInput = new UpdateActivityInput {
            Id = result.AddActivity.Id,
            Description = "Test 2",
            Start = DateTime.Now,
            End = DateTime.Now.AddHours(2)
        };
        // act
        var updateResult = await service.UpdateActivityAsync(updateInput);
        // assert
        Assert.NotNull(updateResult.UpdateActivity);
        Assert.Equal(updateInput.Id, updateResult.UpdateActivity.Id);
        Assert.Equal(updateInput.Description, updateResult.UpdateActivity.Description);
        Assert.Equal(updateInput.Start, updateResult.UpdateActivity.Start);
        Assert.Equal(updateInput.End, updateResult.UpdateActivity.End);
    }
    
}