using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.File
{
    public interface IFileService
    {
        Task<List<string>> SavePhotosAsync(IList<IFormFile> files, string folderName);
        Task DeletePhotosAsync(IList<string> filePaths);
    }
}
