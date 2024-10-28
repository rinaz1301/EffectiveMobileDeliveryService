namespace EffectiveMobileDeliveryService.Service
{
    public class LoggerService : ILoggerService
    {
        IConfiguration _configuration;
        public LoggerService(IConfiguration configuration) 
        { 
            _configuration = configuration;
        }
        public void LogError(string message)
        {
            using(var file = new StreamWriter(_configuration.GetSection("DeliveryLog").Value, true))
            {
                file.WriteLine($"[{DateTime.Now}] Ошибка: {message}.");
            }
        }
        public void LogErrorApi(string method, string message)
        {
            using (var file = new StreamWriter(_configuration.GetSection("DeliveryLog").Value, true))
            {
                file.WriteLine($"[{DateTime.Now}] Метод: {method} Ошибка: {message}.");
            }
        }
        public void LogInformation(string message)
        {
            var df = _configuration.GetSection("DeliveryLog").Value;
            using (var file = new StreamWriter(_configuration.GetSection("DeliveryLog").Value, true))
            {
                file.WriteLine($"[{DateTime.Now}] {message}.");
            }
        }
    }
}
