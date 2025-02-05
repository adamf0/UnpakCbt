using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace UnpakCbt.Common.Presentation.FileManager
{
    public class FileProvider : IFileProvider
    {
        public FileProvider()
        {

        }

        public string GenerateFileName(IFormFile file)
        {
            string fileName = file.FileName;

            return $"{Guid.NewGuid()}_{fileName[..Math.Min(fileName.Length, 50)]}.{GetSafeExtension(file)}";
        }

        public string GetSafeExtension(IFormFile file)
        {
            string fileName = file.FileName;
            int lastDotIndex = fileName.LastIndexOf('.');

            return lastDotIndex != -1 ? GetExtension(fileName[(lastDotIndex + 1)..]) : string.Empty;
        }

        private static string GetExtension(string extension)
        {
            ReadOnlySpan<char> span = extension.AsSpan();
            Span<char> filteredSpan = stackalloc char[span.Length];
            int length = 0;

            foreach (char c in span)
            {
                if (c != '\0' && c != '0')
                {
                    filteredSpan[length++] = c;
                }
            }

            return new string(filteredSpan.Slice(0, length)).ToLowerInvariant();
        }

        public bool IsSafeFileExtension(string extension)
        {
            bool secure = true;
            ReadOnlySpan<char> span = extension.AsSpan();

            foreach (char c in span)
            {
                if (c != '\0' && c != '0')
                {
                    return false;
                }
            }

            return secure;
        }
    }
}
