using AirlineAPI.Data;
using AirlineAPI.Halper;
using AirlineAPI.Interfaces;
using AirlineAPI.Models;
using Newtonsoft.Json;
using System.Text.Json;

namespace AirlineAPI.Services
{
    public class CallbackService : ICallbackService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FileHelper _fileHelper;
        public CallbackService(IHttpContextAccessor httpContextAccessor, FileHelper fileHelper)
        {
            _httpContextAccessor = httpContextAccessor;
            _fileHelper = fileHelper;
        }


        // Dictionary to map IP addresses to types for dynamic deserialization
        private static readonly Dictionary<string, Type> SupportedTypes = new()
        {
            { "::1", typeof(HappyWorldTicketInfoDto) }, // Example IP and type
            // Add more IP-to-type mappings as needed
        };

       

        public void ProcessRequest<T>(T request) where T : class
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request cannot be null.");
            }

            // Example processing: Log the type and serialized content of the request
            Console.WriteLine($"Processing request of type: {typeof(T).Name}");
            Console.WriteLine($"Request content: {System.Text.Json.JsonSerializer.Serialize(request)}");

            // Add your processing logic here
        }


        public string ProcessCallbackRequest(JsonElement callbackRequest)
        {
            if (callbackRequest.ValueKind == JsonValueKind.Undefined || callbackRequest.ValueKind == JsonValueKind.Null)
            {
                return "Invalid payload.";
            }
            try
            {
                // Get the client's IP address
                string? clientIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

                if (string.IsNullOrEmpty(clientIp))
                {
                    return "Client IP is not available.";
                }

                string safeIp = clientIp.Replace(":", "_");
                string currentDate = DateTime.Now.ToString("yyyyMMdd");

                // Look up the type based on the client's IP address
                if (!IpToModelTypeMapper.TryGetRequestType(clientIp, out Type requestType))
                {
                    return $"Unsupported IP: {clientIp}";
                }

                // Convert JsonElement to a JSON string
                string jsonString;
                using (var document = JsonDocument.Parse(callbackRequest.GetRawText()))
                {
                    jsonString = document.RootElement.GetRawText();
                }
                
              

                // Deserialize the JSON to the determined type
              /*  var deserializedRequest = System.Text.Json.JsonSerializer.Deserialize(jsonString, requestType);

                if (deserializedRequest == null)
                {
                    return "Deserialization failed.";
                }

                // Serialize the deserialized request and write to file
                string deserializedJson = JsonConvert.SerializeObject(deserializedRequest);*/


                _fileHelper.ToWriteFile($"{safeIp}_Ticket-res_{currentDate}", "TicketInfo", jsonString, "json");

                return "Callback received successfully.";
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                return $"An error occurred: {ex.Message}";
            }


        }
    }

}