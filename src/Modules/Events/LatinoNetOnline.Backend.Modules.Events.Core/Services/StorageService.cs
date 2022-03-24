
using Azure.Storage.Blobs;

using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using System;
using System.IO;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Services
{
    interface IStorageService
    {
        Task<OperationResult<Uri>> UploadFile(string path, string filename, byte[] file);
    }

    //class GitHubStorageService : IStorageService
    //{
    //    private readonly IGitHubService _githubService;

    //    public GitHubStorageService(IGitHubService githubService)
    //    {
    //        _githubService = githubService ?? throw new ArgumentNullException(nameof(githubService));
    //    }

    //    public async Task<OperationResult<Uri>> UploadFile(string path, string filename, byte[] file)
    //    {
    //        var fileContent = await _githubService.CreateFileAsync(363513177, path, filename, file);

    //        return OperationResult<Uri>.Success(new(fileContent.DownloadUrl));
    //    }
    //}

    class BlobStorageService : IStorageService
    {
        private readonly BlobServiceClient _blobStorageService;

        public BlobStorageService(BlobServiceClient blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        public async Task<OperationResult<Uri>> UploadFile(string path, string filename, byte[] file)
        {
            //Create a unique name for the container
            string containerName = path;

            // Create the container and return a container client object
            BlobContainerClient containerClient = _blobStorageService.GetBlobContainerClient(containerName);


            // Get a reference to a blob
            BlobClient blobClient = containerClient.GetBlobClient(filename);

            Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

            var stream = new MemoryStream(file, writable: false);

            await blobClient.UploadAsync(stream, true);

            return OperationResult<Uri>.Success(blobClient.Uri);


        }
    }
}
