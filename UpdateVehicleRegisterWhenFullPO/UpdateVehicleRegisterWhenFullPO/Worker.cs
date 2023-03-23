using Dapper;
using log4net;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using UpdateVehicleRegisterWhenFullPO.Context;
using UpdateVehicleRegisterWhenFullPO.Models;

namespace UpdateVehicleRegisterWhenFullPO
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IServiceProvider _serviceProvider;
        private WorkerOptions workerOptions;

        public Worker(ILogger<Worker> logger, IServiceProvider services, WorkerOptions workerOptions)
        {
            _logger = logger;
            this.workerOptions = workerOptions;
            _serviceProvider = services ?? throw new ArgumentNullException(nameof(services));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: " + DateTimeOffset.Now);
                using (var scope = _serviceProvider.CreateScope())
                {
                    try
                    {
                        using var BanCan = new SqlConnection(this.workerOptions.BanCanConnection);
                        await BanCan.OpenAsync();
                        using var context = new WebContext();
                        string fnGetSLDaNhapTuPONumber = @"SELECT [dbo].[fnGetSLDaNhapTuPONumber] (
                                                               @PONumber
                                                              ,@ProductCode)
                                                            GO";
                        //lấy list khai theo tàu lên. những line có status = 1
                        var listOfRegisForShip = await context.RegisterForShip
                            .Where(i => i.Status == true)
                            .Select(value => new TempObject
                            {
                                PONumber = value.PONumber,
                                ProductCode = value.ProductCode,
                                ShipNumber = value.ShipNumber
                            })
                            .Distinct()
                            .ToListAsync();
                        _logger.LogInformation("danh sách khai theo tàu: " + ObjectDumper.Dump(listOfRegisForShip, DumpStyle.CSharp));
                        //với mỗi vật tư
                        foreach (var item in listOfRegisForShip)
                        {
                            _logger.LogInformation("PO thứ i: " + ObjectDumper.Dump(item, DumpStyle.CSharp));
                            //kiểm tra vật tư đó nhập đầy chưa
                            //kiểm tra xem nó nhập bao nhiêu
                            var trongLuongDaNhap = (await BanCan.QueryAsync<decimal>(fnGetSLDaNhapTuPONumber,
                                new { item.PONumber, item.ProductCode }, commandType: CommandType.Text))
                                .FirstOrDefault();
                            //trọng lượng yêu cầu
                            var trongLuongYeuCau = await context.PolineModel.Where(i => i.ProductCode == item.ProductCode)
                                                                                         .Where(i => i.Ponumber == item.PONumber)
                                                                                         .Select(e => e.Qty)
                                                                                         .FirstOrDefaultAsync();
                            if (trongLuongDaNhap > trongLuongYeuCau)
                            {
                                //nếu mà đã đầy
                                //po gốc
                                var _1stPO = await context.OrderMapping//.Where(i => i.ShipNumber == item.ShipNumber)
                                                                       .Where(i => i.OrderNumber == item.PONumber)
                                                                       .FirstOrDefaultAsync();
                                //kiểm tra xem có po chung tàu không để chuyển
                                var _2ndPO = await context.OrderMapping.Where(i => i.ShipNumber == item.ShipNumber)
                                                                       .Where(i => i.OrderNumber != item.PONumber)
                                                                       .Where(i => i.IsDone != true)
                                                                       .OrderByDescending(e => e.CreatedTime)
                                                                       .FirstOrDefaultAsync();
                                _logger.LogInformation("PO thứ 2, thay cho PO cũ: " + ObjectDumper.Dump(_2ndPO, DumpStyle.CSharp));
                                //xét còn active không
                                //ngày và số lượng đã giao
                                if (_1stPO.CreatedTime.Value.AddDays(4) < _2ndPO.CreatedTime.Value || _1stPO.CreatedTime.Value.AddDays(-4) > _2ndPO.CreatedTime.Value)
                                {
                                    continue;
                                }
                                //xét chung hầm
                                var isChungHam = await context.RegisterForShip.Where(i => i.PONumber == _2ndPO.OrderNumber)
                                                                                           .ToListAsync();
                                //nếu chưa khai
                                if (isChungHam.Count == 0)
                                {
                                    //lấy list cũ ra cập nhập po mới
                                    var listRegisToUpdate = await context.RegisterForShip.Where(i => i.PONumber == _1stPO.OrderNumber)
                                                                                           .ToListAsync();

                                    var product = await context.PolineModel.Where(i => i.Ponumber == _2ndPO.OrderNumber)
                                                                                    .FirstOrDefaultAsync();
                                    //

                                    //
                                    foreach (var items in listRegisToUpdate)
                                    {
                                        items.PONumber = _2ndPO.OrderNumber;
                                        item.ProductCode = product.ProductCode;
                                    }

                                    _logger.LogInformation("PO thay thế: " + ObjectDumper.Dump(_2ndPO.OrderNumber, DumpStyle.CSharp));
                                    _logger.LogInformation("Mã vật tư thay thế: " + ObjectDumper.Dump(product.ProductCode, DumpStyle.CSharp));

                                    context.UpdateRange(listRegisToUpdate);
                                    await context.SaveChangesAsync();
                                }
                                //đánh dấu vào maping
                                var mapingITem = await context.OrderMapping.Where(i => i.IsDone != true)
                                                                       .Where(i => i.OrderNumber == item.PONumber)
                                                                       .ToListAsync();
                                foreach (var order in mapingITem)
                                {
                                    order.IsDone = true;
                                }
                                context.UpdateRange(mapingITem);
                                await context.SaveChangesAsync();
                                _logger.LogInformation("Hoàn thành!");
                            }
                        }
                        await BanCan.CloseAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        throw;
                    }
                }
                await Task.Delay(180000, stoppingToken);
            }
        }
    }

    public class TempObject
    {
        //số po
        public string PONumber { get; set; }

        //mã hàng
        public string ProductCode { get; set; }

        //số tàu
        public string ShipNumber { get; set; }
    }
}