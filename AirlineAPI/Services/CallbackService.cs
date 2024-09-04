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

            /* try
             {
                 // Get the client's IP address
                 string? clientIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

                 string safeIp = clientIp.Replace(":", "_");
                 string currentDate = DateTime.Now.ToString("yyyyMMdd");
                 _fileHelper.ToWriteFile($"{safeIp}_Ticket-res_{currentDate}", "TicketInfo", JsonConvert.SerializeObject(callbackRequest), "json");

                 if (string.IsNullOrEmpty(clientIp))
                 {
                     return "Client IP is not available.";
                 }

                 // Look up the type based on the client's IP address
                 if (!IpToModelTypeMapper.TryGetRequestType(clientIp, out Type requestType))
                 {
                     return $"Unsupported IP: {clientIp}";
                 }

                 // Deserialize the JSON to the determined type
                 var deserializedRequest = System.Text.Json.JsonSerializer.Deserialize(callbackRequest.GetRawText(), requestType);

                 if (deserializedRequest == null)
                 {
                     return "Deserialization failed.";
                 }

                *//* string safeIp = clientIp.Replace(":", "_");
                 string currentDate = DateTime.Now.ToString("yyyyMMdd");
                 _fileHelper.ToWriteFile($"{safeIp}_Ticket-res_{currentDate}", "TicketInfo", JsonConvert.SerializeObject(deserializedRequest), "json");*//*


                 // Process the deserialized request using a generic method
                 *//* var genericMethod = typeof(ICallbackService).GetMethod(nameof(ProcessRequest));
                  var method = genericMethod!.MakeGenericMethod(requestType);
                  method.Invoke(this, new[] { deserializedRequest });*//*

                 return "Callback received successfully.";
             }*/
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

                // Convert JsonElement to a JSON string
                string jsonString;
                using (var document = JsonDocument.Parse(callbackRequest.GetRawText()))
                {
                    jsonString = document.RootElement.GetRawText();
                }

                // Look up the type based on the client's IP address
                if (!IpToModelTypeMapper.TryGetRequestType(clientIp, out Type requestType))
                {
                    return $"Unsupported IP: {clientIp}";
                }

                // Deserialize the JSON to the determined type
                var deserializedRequest = System.Text.Json.JsonSerializer.Deserialize(jsonString, requestType);

                if (deserializedRequest == null)
                {
                    return "Deserialization failed.";
                }

                // Serialize the deserialized request and write to file
                string deserializedJson = JsonConvert.SerializeObject(deserializedRequest);
                _fileHelper.ToWriteFile($"{safeIp}_Ticket-res_{currentDate}", "TicketInfo", deserializedJson, "json");

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