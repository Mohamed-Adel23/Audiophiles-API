using Audiophiles_API.DTOs;

namespace Audiophiles_API.IServices.IFile
{
    public interface IFileService
    {
        Task<UploadFile> UploadFileToServerAsync(IFormFile file, bool isFile, string folderPath);
    }
}
