namespace App.Servier;

[ExtendObjectType(typeof(Query))]
public class CustomQuery {

    public async Task<List<ActivityDTO>> GetOverlappingActivities(
        [Service] CustomQueryService service
    ) => await service.GetOverlappingActivities();

}