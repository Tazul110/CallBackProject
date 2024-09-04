namespace AirlineAPI.Interfaces
{
    public interface IGenericService
    {
        void ProcessRequest<T>(T request) where T : class;
    }
}
