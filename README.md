# Audacia.Azure
This solution contains 3 Nuget packages which are for the following Azure services.

1. Azure Service Bus - Audacia.Azure.ServiceBus
2. Azure Blob Storage - Audacia.Azure.BlobStorage
3. Azure Storage Queue - Audacia.Azure.StorageQueue

## Usage
Use any of the Nuget packages when you want to connect/ integrate with any of the packages allowing for quick set up of adding, updating and removing objects from Blob storage, consuming and adding to Storage queue.

## Why would you use this any of these packages
The reasons to use any of these packages
1. Quick integration - minimal configuration and set up need with any of the chosen services
2. Flexibility - If in the future additional functionality is need, each of the packages having `interfaces` which will allow for seamless transition from the packages implementation to your Projects custom implementation.
3. Easy of use - If you are not familiar with any of the Azure services all the hard work if done and only configuration is needed ready for you to consume.

## Examples
Look in `tools/Audacia.Azure.Demo` for how to setup and use the each of the packages.
