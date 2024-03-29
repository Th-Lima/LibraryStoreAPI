﻿using LibraryStore.Business.Interfaces;
using LibraryStore.Business.Notifications;
using Microsoft.AspNetCore.Http;

namespace LibraryStore.Business.Services
{
    public class ImageService : IImageService
    {
        private readonly INotifier _notifier;

        public ImageService(INotifier notifier)
        {
            _notifier = notifier;
        }

        public bool UploadFileAndConvertToBase64(string file, string imgName)
        {
            if (string.IsNullOrEmpty(file))
            {
                _notifier.Handle(new Notification("Forneça uma imagem para este produto!"));
                return false;
            }

            var imageDataByteArray = Convert.FromBase64String(file);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imgName);

            if (File.Exists(filePath))
            {
                _notifier.Handle(new Notification("Já existe um arquivo com este nome"));
                return false;
            }

            File.WriteAllBytes(filePath, imageDataByteArray);

            return true;
        }

        public async Task<bool> UploadFileWithIFormFile(IFormFile file, string imgPrefix)
        {
            if(file == null || file.Length <= 0)
            {
                _notifier.Handle(new Notification("Forneça uma imagem para este produto!"));
                return false;
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imgPrefix + file.FileName);

            if (File.Exists(path))
            {
                _notifier.Handle(new Notification("Já existe um arquivo com este nome"));
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return true;
        }
    }
}
