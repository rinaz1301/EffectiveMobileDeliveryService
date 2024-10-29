using Bogus.Bson;
using EffectiveMobileDeliveryService.Models;
using Microsoft.AspNetCore.Builder;
using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;

namespace EffectiveMobileDeliveryServiceTests
{
    public class Tests
    {
        //Проверка на существование файлов или директорий
        [Fact]
        public void FileOrDirectoryExist()
        {
            var builder = WebApplication.CreateBuilder();

            var path1 = builder.Configuration.GetSection("DeliveryLog").Value;
            var path2= builder.Configuration.GetSection("DeliveryOrder").Value;
            var path3 = builder.Configuration.GetSection("DeliveryOrdersInput").Value;
            Assert.True(File.Exists(path1) || Directory.Exists(path1.Remove(path1.LastIndexOf('\\'))));
            Assert.True(File.Exists(path2) || Directory.Exists(path2.Remove(path2.LastIndexOf('\\'))));
            Assert.True(File.Exists(path3) || Directory.Exists(path3.Remove(path3.LastIndexOf('\\'))));
        }
        //Проверка на возможность десериализовать json файл
        [Fact]
        public void DeseriazableInputFile()
        {
            var builder = WebApplication.CreateBuilder();

            var path = builder.Configuration.GetSection("DeliveryOrdersInput").Value;

            var json = "";
            using (var file = new StreamReader(path))
            {
                json = file.ReadToEnd();
            }
            Action action = () =>
            {
                JsonConvert.DeserializeObject<List<DeliveryOrder>>(json);
            };
            var exception = Record.Exception(() => action());
            Assert.Null(exception);
        }
    }
}