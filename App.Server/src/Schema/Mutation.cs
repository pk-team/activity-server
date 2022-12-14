namespace App.Server {

    public class Mutation {

        public async Task<SaveActivityPayload> SaveActivityAsync(
            [Service] ActivityService service,
            SaveActivityInput input
        ) => await service.SaveActivityAsync(input);

        // SaveOrganizatilnAsync
        public async Task<SaveOrganizationPayload> SaveOrganizationAsync(
            [Service] OrganizationService service,
            SaveOrganizationInput input
        ) => await service.SaveOrganizationAsync(input);
        
    }
}