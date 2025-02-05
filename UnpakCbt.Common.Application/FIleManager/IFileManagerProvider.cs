using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnpakCbt.Common.Application.FileManager
{
    public interface IFileManagerProvider
    {
        Task<bool> BucketExists(string name);
        Task CreateBucket(string name);
        Task UploadFile(string bucketName, string fileName, string fileType, long fileLength, Stream fileStream);
        Task<MemoryStream> DownloadFile(string bucketName, string fileName);
        Task DeleteFile(string bucketName, string fileName);
        string GetPublicFileUrl(string bucketName, string objectName);
    }
}
