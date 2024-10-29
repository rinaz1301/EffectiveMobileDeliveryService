using EffectiveMobileDeliveryService.Repository;
using EffectiveMobileDeliveryService.Service;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace EffectiveMobileDeliveryService
{
    public class Program
    {
        static bool CheckFilePath(string path)
        {
            try
            {
                if (File.Exists(path))
                    return true;
                if (Directory.Exists(path) && !Regex.IsMatch(path, "\\.[a-z]+"))
                    return true;
                if(Directory.Exists(path.Remove(path.LastIndexOf('\\'))))
                    return true;
                else
                throw new Exception();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удается найти файл или директорию {path}. Необходимо исправить параметры файла конфигурации." + ex.Message + ex.StackTrace);
                return false;
            }
            
            
        }
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            if (!CheckFilePath(builder.Configuration.GetSection("DeliveryLog").Value))
                return;
            if (!CheckFilePath(builder.Configuration.GetSection("DeliveryOrdersInput").Value))
                return;
            if(!CheckFilePath(builder.Configuration.GetSection("DeliveryOrder").Value))
                return;
            
            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddLogging(options =>
            {
                options.Services.AddSingleton<ILoggerService, LoggerService>();
            });
            builder.Services.AddSingleton<IFileRepository, FileRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}