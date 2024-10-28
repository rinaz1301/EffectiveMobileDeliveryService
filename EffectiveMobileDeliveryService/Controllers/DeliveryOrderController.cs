using Bogus;
using EffectiveMobileDeliveryService.Models;
using EffectiveMobileDeliveryService.Repository;
using EffectiveMobileDeliveryService.Service;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace EffectiveMobileDeliveryService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeliveryOrderController : Controller
    {
        IFileRepository _fileRepository;
        IConfiguration _configuration;
        ILoggerService _loggerService;

        public DeliveryOrderController(IFileRepository fileRepository, IConfiguration configuration, ILoggerService loggerService) 
        { 
            _fileRepository = fileRepository;
            _configuration = configuration;
            _loggerService = loggerService;
        }
        [HttpGet]
        [Route("deliveryOrder.json")]
        public string OpenFile()
        {
            try
            {
                _loggerService.LogInformation("запрос на открытие файла deliveryOrder.json");
                string path = _configuration.GetSection("DeliveryOrder").Value;
                using (var file = new StreamReader(path))
                {
                    return file.ReadToEnd();
                }
            }
            catch(FileNotFoundException ex)
            {
                _loggerService.LogErrorApi(Request.GetDisplayUrl(), ex.Message + ex.StackTrace);
                return null;
            }
        }
        [HttpGet]
        [Route("sortedFile")]
        public HttpStatusCode SortedFile([Required] string _district,[Required] DateTime _firstDateTime)
        {
            try
            {
                _loggerService.LogInformation($"запрос на создание файла deliveryOrder.json с фильтрацией данных по району {_district} и по дате {_firstDateTime}");
                string outPath = _configuration.GetSection("DeliveryOrder").Value;
                var deliveryOrders = _fileRepository.GetDeliveryOrders()
                    .Where(x => x.District == _district)
                    .Where(x => x.Time >= _firstDateTime && x.Time <= Convert.ToDateTime(_firstDateTime).AddMinutes(30));
                using (var file = new StreamWriter(outPath))
                {
                    file.WriteLine(JsonConvert.SerializeObject(deliveryOrders, Formatting.Indented));
                }
                return HttpStatusCode.Created;
            }
            catch(Exception ex)
            {
                _loggerService.LogErrorApi(Request.GetDisplayUrl(),ex.Message + ex.StackTrace);
                return HttpStatusCode.BadRequest;
            }

        }
        [HttpGet]
        [Route("createTestFile")]
        [Obsolete("Метод для создания тестового файла")]
        public HttpStatusCode CreateTestFile()
        {
            try
            {
                _loggerService.LogInformation($"запрос на создание тестового файла с данными");
                var jsonString = _fileRepository.GetJsonDeliveryOrders();
                string path = _configuration.GetSection("DeliveryOrderInput").Value;
                using (var file = new StreamWriter(path))
                {
                    file.WriteLine(jsonString);
                }
                return HttpStatusCode.Created;
            }
            catch (Exception ex)
            {
                _loggerService.LogErrorApi(Request.GetDisplayUrl(), ex.Message + ex.StackTrace);
                return HttpStatusCode.BadRequest;
            }
        }
    }
}
