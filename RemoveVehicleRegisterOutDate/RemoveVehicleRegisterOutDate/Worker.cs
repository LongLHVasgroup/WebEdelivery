using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RemoveVehicleRegisterOutDate.Models;
using RemoveVehicleRegisterOutDate.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RemoveVehicleRegisterOutDate
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly DbHelper dbHelper;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            dbHelper = new DbHelper();
        }


        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                

                // viết xử lý connect và xóa những dòng đăng ký cũ tại đây
                try
                {
                    // Thực thi 1 ngày 1 lần
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    List<VehicleRegisterMobileModel> list = dbHelper.GetOutOfDateList();
                    if(list != null)
                    {
                        list?.ForEach(item =>
                        {
                            dbHelper.RemoveRegisterOutOfDate(item.VehicleRegisterMobileId);
                        });
                        _logger.LogInformation("Removed {count} items before {ngayToiDuKien}",list.Count, DateTime.Today.AddDays(-6));
                    }
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "Have an error");
                }

                await Task.Delay(24*60*60*1000, stoppingToken);
            }
        }
        
        
    }
}
