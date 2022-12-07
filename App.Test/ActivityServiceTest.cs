
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
        var input = new SaveActivityInput {
            Description = "Test",
            Start = DateTime.Now,
            End = DateTime.Now.AddHours(1)
        };
        // act
        var result = await service.SaveActivityAsync(input);
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
        var input = new SaveActivityInput {
            Description = "Test",
            Start = DateTime.Now,
            End = DateTime.Now.AddHours(1)
        };
        var result = await service.SaveActivityAsync(input);
        var updateInput = new SaveActivityInput {
            Id = result.AddActivity.Id,
            Description = "Test 2",
            Start = DateTime.Now,
            End = DateTime.Now.AddHours(2)
        };
        // act
        var updateResult = await service.SaveActivityAsync(updateInput);
        // assert
        Assert.NotNull(updateResult.AddActivity);
        Assert.Equal(updateInput.Id, updateResult.AddActivity.Id);
        Assert.Equal(updateInput.Description, updateResult.AddActivity.Description);
        Assert.Equal(updateInput.Start, updateResult.AddActivity.Start);
        Assert.Equal(updateInput.End, updateResult.AddActivity.End);
        // assert duration minutes is correct
        Assert.Equal((int)(updateInput.End - updateInput.Start).Value.TotalMinutes, updateResult.AddActivity.DurationMinutes);
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
}