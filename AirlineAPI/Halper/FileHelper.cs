using AirlineAPI.Data;
using AirlineAPI.Interfaces;
using System.Xml.Serialization;

namespace AirlineAPI.Halper
{
    public class FileHelper
    {
        private readonly AwsS3Settings _s3Settings;
        private readonly IS3Service _s3Service;
        public FileHelper(
                AwsS3Settings s3Settings,
                IS3Service s3Service
            )
        {
            _s3Settings = s3Settings;
            _s3Service = s3Service;
        }
        public T? DeserializeXMLFileToObject<T>(string xml)
        {
            T? returnObject = default;
            if (string.IsNullOrEmpty(xml)) return default;

            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using (TextReader reader = new StringReader(xml))
                {
                    returnObject = (T)serializer.Deserialize(reader)!;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, DateTime.Now);
            }
            return returnObject;
        }

        public string SerializeObjectToXML<T>(T obj)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using (StringWriter writer = new StringWriter())
                {
                    serializer.Serialize(writer, obj);
                    return writer.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, DateTime.Now);
                return "";
            }
        }

        public async Task<string> toReadFile(string filepath)
        {
            var s3Result = _s3Settings.IsLive ?
                    await _s3Service.ReadObjectFromBucketAsync(_s3Settings.BucketName, filepath, "") :
                    (false, string.Empty, string.Empty);

            if (s3Result.Item1)
            {
                using TextReader reader = new StringReader(s3Result.Item2);
                return reader.ReadToEnd();
            }
            else
            {
                return File.ReadAllText(filepath);
            }
        }
        public async Task ToWriteFile(string fileName, string folderName, string content, string fileType)
        {
            string path = Path.Combine(Environment.CurrentDirectory, folderName, $"{fileName}.{fileType}");

            if (!Directory.Exists(path))
            {
                string? dirPath = Path.GetDirectoryName(path);
                if (dirPath == null) throw new InvalidOperationException("Failure to save local security settings");
                if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
            }

            File.WriteAllText(path, content);

            if (_s3Settings.IsLive)
            {
                bool uploadTask = await _s3Service.UploadFileAsync(_s3Settings.BucketName, $"{folderName}/{fileName}.xml", path);
                if (uploadTask)
                {
                    File.Delete(path);
                }
            }
        }
    }
}
