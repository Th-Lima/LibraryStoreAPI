using LibraryStore.Business.Interfaces;
using LibraryStore.Business.Notifications;

namespace LibraryStore.Business.Services
{
    public class ImageService : IImageService
    {
        private readonly INotifier _notifier;

        public ImageService(INotifier notifier)
        {
            _notifier = notifier;
        }

        public bool UploadFile(string file, string imgName)
        {
            var imageDataByteArray = Convert.FromBase64String(file);

            if (string.IsNullOrEmpty(file))
            {
                _notifier.Handle(new Notification("Forneça uma imagem para este produto!"));
                return false;
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imgName);

            if (File.Exists(filePath))
            {
                _notifier.Handle(new Notification("Já existe um arquivo com este nome"));
                return false;
            }

            File.WriteAllBytes(filePath, imageDataByteArray);

            return true;
        }
    }
}
