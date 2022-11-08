using Microsoft.AspNetCore.Http;

namespace LibraryStore.Business.Interfaces
{
    public interface IImageService
    {
        public bool UploadFileAndConvertToBase64(string file, string imgName);
        public Task<bool> UploadFileWithIFormFile(IFormFile file, string imgPrefix);
    }
}
