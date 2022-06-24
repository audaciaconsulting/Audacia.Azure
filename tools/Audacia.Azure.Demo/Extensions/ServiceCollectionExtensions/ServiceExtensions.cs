using Audacia.Azure.BlobStorage.AddBlob;
using Audacia.Azure.BlobStorage.DeleteBlob;
using Audacia.Azure.BlobStorage.GetBlob;
using Audacia.Azure.BlobStorage.UpdateBlob;
using Audacia.Azure.StorageQueue.AddMessageToQueue;
using Microsoft.Extensions.DependencyInjection;

namespace Audacia.Azure.Demo.Extensions.ServiceCollectionExtensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddAzureBlobServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddScoped<IGetAzureBlobStorageService, GetAzureBlobStorageService>()
                .AddScoped<IAddAzureBlobStorageService, AddAzureBlobStorageService>()
                .AddScoped<IUpdateAzureBlobStorageService, UpdateAzureBlobStorageService>()
                .AddScoped<IDeleteAzureBlobStorageService, DeleteAzureBlobStorageService>();
        }

        public static IServiceCollection AddAzureQueueServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddScoped<IAddAzureQueueStorageService, AddAzureQueueStorageService>();
        }
    }
}
