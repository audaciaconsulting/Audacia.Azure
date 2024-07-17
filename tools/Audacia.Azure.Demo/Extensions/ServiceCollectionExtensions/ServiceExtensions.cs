using Audacia.Azure.BlobStorage.AddBlob;
using Audacia.Azure.BlobStorage.DeleteBlob;
using Audacia.Azure.BlobStorage.GetBlob;
using Audacia.Azure.BlobStorage.UpdateBlob;
using Audacia.Azure.StorageQueue.AddMessageToQueue;
using Audacia.Azure.StorageQueue.DeleteMessageFromQueue;
using Audacia.Azure.StorageQueue.GetMessages;

namespace Audacia.Azure.Demo.Extensions.ServiceCollectionExtensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddAzureBlobServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddScoped<IGetAzureBlobStorageService, GetAzureBlobStorageService>()
            .AddScoped<IGetAzureProtectedBlobStorageService, GetAzureProtectedBlobStorageService>()
            .AddScoped<IAddAzureBlobStorageService, AddAzureBlobStorageService>()
            .AddScoped<IUpdateAzureBlobStorageService, UpdateAzureBlobStorageService>()
            .AddScoped<IDeleteAzureBlobStorageService, DeleteAzureBlobStorageService>();
    }

        public static IServiceCollection AddAzureQueueServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddScoped<IGetAzureQueueStorageService, GetAzureQueueStorageService>()
            .AddScoped<IAddAzureQueueStorageService, AddAzureQueueStorageService>()
            .AddScoped<IDeleteAzureQueueStorageService, DeleteAzureQueueStorageService>();
    }
    }
}
