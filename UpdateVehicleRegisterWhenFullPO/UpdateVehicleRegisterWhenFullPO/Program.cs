using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using UpdateVehicleRegisterWhenFullPO.Context;
using UpdateVehicleRegisterWhenFullPO.Contracts;
using UpdateVehicleRegisterWhenFullPO.Contracts.Web;
using UpdateVehicleRegisterWhenFullPO.Models;
using UpdateVehicleRegisterWhenFullPO.Repositories;
using UpdateVehicleRegisterWhenFullPO.Repositories.Web;

namespace UpdateVehicleRegisterWhenFullPO
{
    public class Program
    {
        private static WorkerOptions workerOptions;
        private static string origAssemblyLocation = AppDomain.CurrentDomain.BaseDirectory;

        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(origAssemblyLocation + "\\LogFile.txt")
                .CreateLogger();

            try
            {
                Log.Information("Starting up the service");
                CreateHostBuilder(args).Build().Run();
                return;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "There was a problem starting the serivce");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;

                    workerOptions = configuration.GetSection("ConnectionStrings").Get<WorkerOptions>();

                    Config.getInstance().connWeb = workerOptions.WebConnection;

                    services.AddScoped(typeof(IRepositoryBase<>), typeof(BaseRepository<>));
                    //services.AddScoped<IOrderMappingRepository, OrderMappingRepository>();
                    services.AddScoped<IOrderMappingRepository, OrderMappingRepository>();

                    services.AddDbContext<WebContext>(
                        options => options.UseSqlServer(workerOptions.WebConnection),
                                                      ServiceLifetime.Scoped
                    );

                    //services.AddSingleton<OrderMappingRepository>();
                    services.AddSingleton(workerOptions);
                    services.AddHostedService<Worker>();
                }).UseSerilog();
    }
}