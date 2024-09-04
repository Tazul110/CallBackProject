using System.Linq;
using System.Net;

namespace AirlineAPI.Middlewire
{
    public class IpWhitelistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly List<IPAddress> _allowedIPs;
        private readonly List<IPNetwork> _allowedIPNetworks;
        private readonly string _restrictedPath = "/api/callback";

        public IpWhitelistMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;

            // Fetch allowed IPs from configuration
            var allowedIpConfigs = configuration.GetSection("AllowedIPs").Get<List<string>>() ?? new List<string>();

            // Parse and validate IP addresses
            _allowedIPs = new List<IPAddress>();
            foreach (var ip in allowedIpConfigs.Where(ip => !ip.Contains("/")))
            {
                if (IPAddress.TryParse(ip, out var parsedIp))
                {
                    _allowedIPs.Add(parsedIp);
                }
                else
                {
                    // Log invalid IP address format
                    Console.WriteLine($"Invalid IP address format: {ip}");
                }
            }

            // Parse and validate CIDR notations
            _allowedIPNetworks = new List<IPNetwork>();
            foreach (var cidr in allowedIpConfigs.Where(ip => ip.Contains("/")))
            {
                try
                {
                    var network = IPNetwork.Parse(cidr);
                    _allowedIPNetworks.Add(network);
                }
                catch (Exception ex)
                {
                    // Log invalid CIDR format
                    Console.WriteLine($"Invalid CIDR format: {cidr}. Error: {ex.Message}");
                }
            }
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments(_restrictedPath))
            {
                var remoteIp = context.Connection.RemoteIpAddress;

                if (remoteIp != null && (_allowedIPs.Contains(remoteIp) || _allowedIPNetworks.Any(network => network.Contains(remoteIp))))
                {
                    await _next(context);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Forbidden: Your IP address is not allowed.");
                }
            }
            else
            {
                // If the path does not match, just continue to the next middleware
                await _next(context);
            }
        }
    }
}
