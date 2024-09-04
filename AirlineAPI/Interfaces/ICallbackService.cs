using System.Text.Json;

namespace AirlineAPI.Interfaces
{
    public interface ICallbackService
    {
        void ProcessRequest<T>(T request) where T : class;
       public string ProcessCallbackRequest(JsonElement callbackRequest);
    }
}
