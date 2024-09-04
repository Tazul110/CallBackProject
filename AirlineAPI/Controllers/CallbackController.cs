using AirlineAPI.Interfaces;
using AirlineAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


namespace AirlineAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CallbackController : ControllerBase
    {
        private readonly IGenericService _genericService;

        // Dictionary to map PC IP addresses to types for dynamic deserialization
        private static readonly Dictionary<string, Type> SupportedTypes = new()
        {
            { "::1", typeof(TicketInfoDto) }, // Example IP and type
            // Add more IP-to-type mappings as needed
            // { "192.168.1.2", typeof(OtherDto) }
        };

        public CallbackController(IGenericService genericService)
        {
            _genericService = genericService;
        }

        [HttpPost("callback")]
        public IActionResult TicketCallback([FromBody] JsonElement callbackRequest)
        {
            if (callbackRequest.ValueKind == JsonValueKind.Undefined || callbackRequest.ValueKind == JsonValueKind.Null)
            {
                return BadRequest("Invalid payload.");
            }

            try
            {
                // Get the client's IP address
                string clientIp = HttpContext.Connection.RemoteIpAddress!.ToString();

                // Look up the type based on the client's IP address
                if (!SupportedTypes.TryGetValue(clientIp, out Type requestType))
                {
                    return BadRequest($"Unsupported IP: {clientIp}");
                }

                // Deserialize the JSON to the determined type
                var deserializedRequest = JsonSerializer.Deserialize(callbackRequest.GetRawText(), requestType);

                if (deserializedRequest == null)
                {
                    return BadRequest("Deserialization failed.");
                }

                var genericMethod = typeof(IGenericService).GetMethod(nameof(IGenericService.ProcessRequest));
                var method = genericMethod!.MakeGenericMethod(requestType);
                method.Invoke(_genericService, new[] { deserializedRequest });

                return Ok(new { message = "Callback received successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}