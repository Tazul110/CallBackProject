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
        private readonly ICallbackService _callbackService;

        // Dictionary to map PC IP addresses to types for dynamic deserialization
       /* private static readonly Dictionary<string, Type> SupportedTypes = new()
        {
            { "::1", typeof(TicketInfoDto) }, // Example IP and type
            // Add more IP-to-type mappings as needed
            // { "192.168.1.2", typeof(OtherDto) }
        };*/

        public CallbackController(ICallbackService callbackService)
        {
            _callbackService = callbackService;
        }

        [HttpPost("callback")]
        public IActionResult TicketCallback([FromBody] JsonElement callbackRequest)
        {
            var result = _callbackService.ProcessCallbackRequest(callbackRequest);

            if (result == "Callback received successfully.")
            {
                return Ok(new { message = result });
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}