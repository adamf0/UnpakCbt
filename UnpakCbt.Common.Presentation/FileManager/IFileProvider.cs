using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnpakCbt.Common.Presentation.FileManager
{
    public interface IFileProvider
    {
        bool IsSafeFileExtension(string extension);
        string GenerateFileName(IFormFile file);
        string GetSafeExtension(IFormFile file);
    }
}
