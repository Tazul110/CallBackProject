using AirlineAPI.Interfaces;

namespace AirlineAPI.Services
{
    public class GenericService: IGenericService
    {
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
    }
}
