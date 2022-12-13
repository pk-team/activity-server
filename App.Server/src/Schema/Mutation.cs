namespace App.Server {

    public class Mutation {

        public async Task<SaveActivityPayload> SaveActivityAsync(
            [Service] ActivityService service,
            SaveActivityInput input
        ) => await service.SaveActivityAsync(input);

    }
}