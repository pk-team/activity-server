namespace App.Server {

    public class Mutation {

        public async Task<SaveActivityPayload> SaveActivityAsync(
            [Service] ActivityService service,
            SaveActivityInput input
        ) => await service.SaveActivityAsync(input);

        public async Task<SaveOrganizationPayload> SaveOrganizationAsync(
            [Service] OrganizationService service,
            SaveOrganizationInput input
        ) => await service.SaveOrganizationAsync(input);

        public async Task<BulkSaveOrganizationsPayload> BulkSaveOrganizationsAsync(
            [Service] OrganizationService service,
            BulkSaveOrganizationsInput input
        ) => await service.BulkSaveOrganizationsAsync(input);

        
        public async Task<BulkSaveActivitiesPayload> BulkSaveActivitiesAsync(
            [Service] ActivityService service,
            BulkSaveActivitiesInput input
        ) => await service.BulkSaveActivitiesAsync(input);
    }
}