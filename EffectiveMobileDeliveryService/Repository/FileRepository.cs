using Bogus;
using EffectiveMobileDeliveryService.Models;
using EffectiveMobileDeliveryService.Service;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EffectiveMobileDeliveryService.Repository
{
    public class FileRepository : IFileRepository
    {
        IConfiguration _configuration;
        ILoggerService _loggerService;
        
        public FileRepository(IConfiguration configuration, ILoggerService loggerService)
        {
            _configuration = configuration;
            _loggerService = loggerService;
        }
        public List<DeliveryOrder> GetDeliveryOrders()
        {
            try
            {
                string inPath = _configuration.GetSection("DeliveryOrdersInput").Value;
                string json = System.IO.File.ReadAllText(inPath);
                return JsonConvert.DeserializeObject<List<DeliveryOrder>>(json);
            }
            catch(FileNotFoundException ex)
            {
                _loggerService.LogError("Не удалось найти файл DeliveryOrdersInput.json");
                return null;
            }
        }
        public string GetJsonDeliveryOrders()
        {
            Faker<DeliveryOrder> faker = new Faker<DeliveryOrder>()
                .RuleFor(x => x.OrderId, x => x.UniqueIndex)
                .RuleFor(x => x.Weight, x => Math.Round(x.Random.Double(0, 100), 2))
                .RuleFor(x => x.District, x => GetRandomDistrict())
                .RuleFor(x => x.Time, x => x.Date.Between(new DateTime(2024, 10, 1), DateTime.Now));

            return JsonConvert.SerializeObject(faker.Generate(100000).ToList(), Formatting.Indented);
        }
        private string GetRandomDistrict()
        {
            Dictionary<int, string> districts = new Dictionary<int, string>()
            {
                { 0, "Авиастроительный" },
                { 1, "Вахитовский" },
                { 2, "Кировский" },
                { 3, "Московский" },
                { 4, "Ново-Савиновский" },
                { 5, "Приволжский" },
                { 6, "Советский" }
            };
            return districts.GetValueOrDefault(new Random().Next(districts.Count));
        }
    }
}
