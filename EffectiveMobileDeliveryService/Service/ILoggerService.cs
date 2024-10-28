namespace EffectiveMobileDeliveryService.Service
{
    public interface ILoggerService
    {
        void LogError(string message);
        void LogInformation(string message);
        void LogErrorApi(string method, string message);
    }
}
