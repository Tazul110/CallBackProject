namespace AirlineAPI.Data
{
    public class AwsS3Settings
    {
        public string AccessKeyId { get; set; } = String.Empty;
        public string SecretAccessKey { get; set; } = String.Empty;
        public string BucketName { get; set; } = String.Empty;
        public bool IsLive { get; set; }
    }
}
