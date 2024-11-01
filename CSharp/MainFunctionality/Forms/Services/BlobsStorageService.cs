using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;


namespace FormsClone.CSharp.MainFunctionality.Forms.Services
{
    public class BlobService
    {
        private readonly BlobServiceClient _blobServiceClient; // Класс клиента для работы с Azure Blob Storage.
        private readonly string _containerName = "iwillcreateitlater"; // Имя контейнера, в который будут загружаться изображения.

        // Конструктор, принимающий строку подключения к Azure Blob Storage.
        public BlobService(string connectionString)
        {
            _blobServiceClient = new BlobServiceClient(connectionString); // Инициализация клиента службы блоба с использованием строки подключения.
        }

        // Асинхронный метод для загрузки изображения в контейнер.
        public async Task<string> UploadImageAsync(Stream imageStream, string imageName)
        {
            // Получаем клиента для работы с указанным контейнером в Blob Storage.
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync(); // Создаём контейнер, если он еще не существует.

            // Получаем клиента для работы с конкретным блобом (файлом) по имени.
            var blobClient = containerClient.GetBlobClient(imageName);
            await blobClient.UploadAsync(imageStream, new BlobHttpHeaders { ContentType = "image/jpeg" }); // Загружаем изображение и устанавливаем тип контента.

            // Возвращаем URI загруженного изображения.
            return blobClient.Uri.ToString();
        }
    }
}
