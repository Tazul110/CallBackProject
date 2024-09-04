namespace AirlineAPI.Interfaces
{
    public interface IS3Service
    {
        Task<bool> CopyObjectInBucketAsync(string bucketName, string objectName, string folderName);
        Task<(bool, string)> CreateBucketAsync(string bucketName);
        Task<bool> DeleteBucketAsync(string bucketName);
        Task<bool> DeleteBucketContentsAsync(string bucketName);
        Task<bool> DownloadObjectFromBucketAsync(string bucketName, string objectName, string filePath);
        Task<(bool, string, string)> ReadObjectFromBucketAsync(string bucketName, string objectName, string filePath);
        Task<bool> ListBucketContentsAsync(string bucketName);
        Task<bool> UploadFileAsync(string bucketName, string objectName, string filePath);
    }
}
