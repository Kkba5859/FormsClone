using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;


namespace FormsClone.CSharp.MainFunctionality.Forms.Services
{
    public class BlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName = "iwillcreateitlater";

        public BlobService(string connectionString)
        {
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task<string> UploadImageAsync(Stream imageStream, string imageName)
        {
           
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync();

            
            var blobClient = containerClient.GetBlobClient(imageName);
            await blobClient.UploadAsync(imageStream, new BlobHttpHeaders { ContentType = "image/jpeg" });

            
            return blobClient.Uri.ToString();
        }
    }
}