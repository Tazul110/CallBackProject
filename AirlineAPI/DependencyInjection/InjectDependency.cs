using AirlineAPI.Data;
using AirlineAPI.Halper;
using AirlineAPI.Interfaces;
using AirlineAPI.Services;
using Amazon;
using Amazon.S3;

namespace AirlineAPI.DependencyInjection
{
    public static class InjectDependency
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection Services)
        {
           
            // Helper 
            Services.AddScoped<FileHelper>();

            // amazon service 
            Services.AddScoped<IS3Service, S3Service>(provider =>
            {
                var setting = provider.GetService<AwsS3Settings>();
                return new S3Service(new AmazonS3Client(setting?.AccessKeyId,
                                                        setting?.SecretAccessKey,
                                                        RegionEndpoint.APSoutheast1));
            });

            // Service Layer
           

            return Services;

        }
    }
}

