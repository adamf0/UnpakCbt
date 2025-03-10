﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;
using UnpakCbt.Common.Application.FileManager;

namespace UnpakCbt.Common.Infrastructure.FileManager
{
    internal class FileManagerProvider : IFileManagerProvider
    {
        private readonly IMinioClient client;
        private readonly string _endpoint;

        public FileManagerProvider(IMinioClient client, IConfiguration configuration)
        {
            client = client ?? throw new ArgumentNullException(nameof(client));
            _endpoint = configuration["Minio:Endpoint"] ?? throw new ArgumentNullException("Minio:Endpoint");
        }

        public async Task<bool> BucketExists(string name)
        {
            return await client.BucketExistsAsync(
                    new BucketExistsArgs()
                        .WithBucket(name)
                );
        }

        public async Task CreateBucket(string name)
        {
            await client.MakeBucketAsync(
                    new MakeBucketArgs()
                        .WithBucket(name)
                );
        }

        public async Task UploadFile(string bucketName, string fileName, string fileType, long fileLength, Stream fileStream)
        {
            await client.PutObjectAsync(
                new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(fileName)
                    .WithStreamData(fileStream)
                    .WithObjectSize(fileLength)
                    .WithContentType(fileType)
            );
        }

        public async Task<MemoryStream> DownloadFile(string bucketName, string fileName)
        {
            var stream = new MemoryStream();
            var tsc = new TaskCompletionSource<bool>();

            var getObjectArgs = new GetObjectArgs()
                .WithBucket(bucketName.ToLower())
                .WithObject(fileName.ToLower())
                .WithCallbackStream(cs =>
                {
                    cs.CopyTo(stream);
                    tsc.SetResult(true);
                });

            await client.GetObjectAsync(getObjectArgs);
            await tsc.Task;
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public async Task DeleteFile(string bucketName, string fileName)
        {
            await client.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(bucketName.ToLower())
                .WithObject(fileName.ToLower())
            );
        }

        public string GetPublicFileUrl(string bucketName, string objectName)
        {
            return $"{_endpoint}/{bucketName}/{objectName}";
        }
    }
}
