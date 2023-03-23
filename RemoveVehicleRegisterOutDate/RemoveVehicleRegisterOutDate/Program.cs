using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RemoveVehicleRegisterOutDate.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoveVehicleRegisterOutDate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseWindowsService() // dùng như 1 windows service
            .UseSerilog() // dùng ghi file log
                .ConfigureServices((hostContext, services) =>
                {
                    Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(hostContext.Configuration).CreateLogger();

                    IConfiguration configuration = hostContext.Configuration;
                    AppSetting.Configuration = configuration;
                    AppSetting.ConnectionString = configuration.GetConnectionString("DefaultConnection");

                    var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
                    optionBuilder.UseSqlServer(AppSetting.ConnectionString);
                     
                    services.AddHostedService<Worker>();
                });
    }
}
