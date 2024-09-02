# Audacia.Azure

This solution contains 2 Nuget packages which are for the following Azure services.

1. Azure Blob Storage - Audacia.Azure.BlobStorage
2. Azure Storage Queue - Audacia.Azure.StorageQueue

There is also a `Common` package used by `Audacia.Azure.BlobStorage` allowing extension of the `ReturnOptions` interfaces/ classes for custom implementations with adding the dependencies of `Audacia.Azure.BlobStorage`.

1. Azure Common - Audacia.Azure.Common

## Usage

Use any of the Nuget packages when you want to connect/ integrate with any of the packages allowing for quick set up of adding, updating and removing objects from Blob storage, consuming and adding to Storage queue.

### Setting Up an Azure Storage Account

1. **Sign in to the Azure Portal:**
   - Open your web browser and go to the [Azure Portal](https://portal.azure.com).
   - Sign in with your Azure account credentials.

2. **Create a Storage Account:**
   - In the Azure Portal, select **Create a resource** from the left-hand menu.
   - Search for **Storage account** in the search bar and select **Storage account - blob, file, table, queue**.
   - Click **Create** to begin the storage account creation process.

3. **Configure Storage Account Settings:**
   - Under the **Basics** tab:
     - **Subscription:** Select the appropriate subscription.
     - **Resource Group:** Select an existing resource group or create a new one.
     - **Storage Account Name:** Enter a unique name for your storage account (e.g., `mystorageaccount`). This name must be globally unique, use only lowercase letters and numbers, and be between 3 and 24 characters long.
     - **Region:** Choose a region closest to your users.
     - **Performance:** Choose either **Standard** or **Premium** based on your performance needs.
     - **Redundancy:** Choose a redundancy option (e.g., **Locally-redundant storage (LRS)**, **Geo-redundant storage (GRS)**).
   - Click **Next: Networking** to continue.

4. **Configure Networking, Data Protection, and Advanced Settings:**
   - Leave the default options or configure them according to your requirements.
   - Click **Review + Create** after filling in all necessary details.
   - Click **Create** to deploy your storage account.

5. **Access the Storage Account:**
   - Once the deployment is complete, navigate to open your newly created storage account.

6. **Get the Storage Account Name and Access Key:**
   - In the storage account page, navigate to **Settings** > **Access keys**.
   - You will see two access keys (`key1` and `key2`) and their associated **Connection strings**.
   - Note down the **Storage Account Name** and **Access Key** (copy `key1` or `key2`).

7. **Configure Connection Information:**
   - Using a tool like **Azure Storage Explorer** you can view the information required for your application config when using this package.

   - Use and enter the following information within your ***application configuration*** to connect to your local Azurite storage:

   - ***Azure:BlobStorageConfig:***
        - ***AccountName:*** `Storage Account Name` set within step 3.
        - ***AccessKey:*** `Access Key` from step 6.
        - ***BlobEndpoint:*** `Leave empty when using azure hosted storage account`

   - ***Azure:QueueStorageConfig:***
        - ***AccountName:*** `Storage Account Name` set within step 3.
        - ***AccessKey:*** `Access Key` from step 6.
        - ***QueueEndpoint:*** `Leave empty when using azure hosted storage account`

##### Example:

- **Storage Account Name:** `mystorageaccount`
- **Access Key:** `xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx`

7. **Save Your Access Information Securely:**
   - Save your **Storage Account Name** and **Access Key** securely. This information will be required to connect to your storage account via the Audacia.Azure library.

> **Note:** Treat the access key like a password. Do not share it publicly and store it in a secure location.

---

### Setting Up a Local Azure Storage Account

To simulate Azure Storage services on your local machine, you can use **Azurite**, an open-source emulator for Azure Storage. This is useful for development and testing purposes.

#### Step-by-Step Guide to Set Up a Local Storage Account

1. **Install Azurite:**
   - Azurite is available as an npm package. You need to have **Node.js** installed on your machine. If you don't have Node.js, download and install it from the [Node.js Official Site](https://nodejs.org).
   - Open your terminal or command prompt and run the following command to install Azurite globally:  
   `"npm install -g azurite"`

2. **Start Azurite:**
   - After installing Azurite, start the Azurite server by running the following command:  
   `"azurite --silent --location c:\azurite --debug c:\azurite\debug.log"`
        - **Data Persistence:** By default, Docker containers are ephemeral, which means any data stored inside them is lost when the container is stopped or deleted. Specifying a local path ensures that your data persists beyond the lifecycle of the container.
        - **Development and Debugging:** Storing Azurite data locally makes it easier to view and manipulate the files directly, which can be helpful for debugging or data inspection during development.
   - By default, Azurite will start listening for:
     - **Blob service** on `"http://127.0.0.1:10000"`
     - **Queue service** on `"http://127.0.0.1:10001"`
     - **Table service** on `"http://127.0.0.1:10002"`

3. **Verify the Azurite Installation:**
   - To ensure Azurite is running correctly, you can use a tool like **Azure Storage Explorer** to manage and view.

4. **Configure Connection Information:**
   - Using a tool like **Azure Storage Explorer** you can view the information required for your application config when using this package.

   - Use and enter the following information within your ***application configuration*** to connect to your local Azurite storage:

   - ***Azure:BlobStorageConfig:***
        - ***AccountName:*** `By default "devstoreaccount1"`
        - ***AccessKey:*** `Found within Azure Storage Explorer under the name "Primary Key"`
        - ***BlobEndpoint:*** `default is "http://127.0.0.1:10000/{Account Name}"`

   - ***Azure:QueueStorageConfig:***
        - ***AccountName:*** `By default "devstoreaccount1"`
        - ***AccessKey:*** `Found within Azure Storage Explorer under the name "Primary Key"`
        - ***QueueEndpoint:*** `default is "http://127.0.0.1:10001/{Account Name}"`

> **Note:** Azurite is designed for development and testing purposes only. It should not be used in a production environment.

## Why would you use this any of these packages

The reasons to use any of these packages

1. Quick integration - minimal configuration and set up need with any of the chosen services
2. Flexibility - If in the future additional functionality is need, each of the packages having `interfaces` which will allow for seamless transition from the packages implementation to your Projects custom implementation.
3. Easy of use - If you are not familiar with any of the Azure services all the hard work if done and only configuration is needed ready for you to consume.

## Examples

Look in `tools/Audacia.Azure.Demo` for how to setup and use the each of the packages.

Look in `SETUP.md` for how to setup local and Azure hosted storage accounts.

## Contributing

We welcome contributions! Please feel free to check our [Contribution Guidlines](https://github.com/audaciaconsulting/.github/blob/main/CONTRIBUTING.md) for feature requests, issue reporting and guidelines.
