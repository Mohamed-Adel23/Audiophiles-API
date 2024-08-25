using Audiophiles_API.DTOs;
using Audiophiles_API.IServices.IFile;

namespace Audiophiles_API.Services.File
{
    public class FileService : IFileService
    {
        private string[] _allowedFileExtensions = { ".pdf", ".txt", ".doc", ".docx" };
        private int _maxFileSize = 1 * 1024 * 1024 * 1024; // 1 GB
        private string[] _allowedImageExtensions = { ".jpg", ".png", ".jpeg" };
        private int _maxImageSize = 5 * 1024 * 1024; // 3 MB

        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<UploadFile> UploadFileToServerAsync(IFormFile file, bool isFile, string folderPath)
        {
            var result = new UploadFile();
            // Check The Extension 
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (isFile)
            {
                if (!_allowedFileExtensions.Contains(fileExtension))
                {
                    result.Message = $"Invalid file extension. Allowed extensions are ({string.Join(",", _allowedFileExtensions)})";
                    return result;
                }
                // Check The Size 
                if (file.Length > _maxFileSize)
                {
                    result.Message = $"The file size exceeds the allowed limit. You must upload files smaller than or equal to {_maxFileSize / (1024 * 1024 * 1024)} GB";
                    return result;
                }
            }
            else
            {
                if (!_allowedImageExtensions.Contains(fileExtension))
                {
                    result.Message = $"Invalid file extension. Allowed extensions are ({string.Join(",", _allowedImageExtensions)})";
                    return result;
                }
                // Check The Size 
                if (file.Length > _maxImageSize)
                {
                    result.Message = $"The file size exceeds the allowed limit. You must upload files smaller than or equal to {_maxImageSize / (1024 * 1024 * 1024)} GB";
                    return result;
                }
            }

            var fileName = file.FileName;
            // Create New Fake Name for the file
            fileName = $"{Guid.NewGuid().ToString().Substring(0, 15)}-AudioPhile{Path.GetExtension(fileName).ToLowerInvariant()}";
            // Get The Actual Path to store on server
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, folderPath, fileName);
            // Move The File
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);

            result.FileName = fileName;

            return result;
        }
    }
}
