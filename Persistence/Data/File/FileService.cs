using Application.Interfaces.File;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace Persistence.Data.File
{
    public class FileService(
        IWebHostEnvironment env
        ) : IFileService
    {
        public async Task<List<string>> SavePhotosAsync(IList<IFormFile> files, string folderName)
        {

            var filePaths = new List<string>();
            string rootPath = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string uploadPath = Path.Combine(rootPath, "Photos", folderName);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    // Getting file extension
                    string extension = Path.GetExtension(file.FileName);

                    // Creating unique file name
                    string newFileName = $"{Guid.NewGuid()}{extension}";

                    // Path to save
                    string fullPath = Path.Combine(uploadPath, newFileName);

                    // Saving the file
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var dbPath = Path.Combine("Photos", folderName, newFileName).Replace("\\", "/");
                    filePaths.Add(dbPath);
                }
            }
            return filePaths;
        }
        public Task DeletePhotosAsync(IList<string> filePaths)
        {
            if (filePaths == null || !filePaths.Any())
                return Task.CompletedTask;
            foreach (var filePath in filePaths) 
            {
                if(File.Exists(filePath))

            }
            throw new NotImplementedException();
        }

    }
}
