using AirlineAPI.Models;

namespace AirlineAPI.Data
{
    public static class IpToModelTypeMapper
    {
        private static readonly Dictionary<string, Type> _IpToModel = new()
        {
            { "::1", typeof(HappyWorldTicketInfoDto) }, // Example IP and type
            // Add more IP-to-type mappings as needed
        };

        public static bool TryGetRequestType(string ip, out Type requestType)
        {
            return _IpToModel.TryGetValue(ip, out requestType);
        }
    }
}
