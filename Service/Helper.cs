using ApiTest.Common;
using Service.ADO;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Dapper;
using AutoMapper;

namespace Service
{
    public class SubTicket
    {
        public string MainCode { get; set; }
        public string SubCode { get; set; }
        public string InvalidSAPMessage { get; set; }
        public decimal? MainWeight { get; set; }
        public decimal? MainReduce { get; set; }
        public decimal? SubWeight { get; set; }
        public decimal? SubReduce { get; set; }
        public string MainVehicleNumber { get; set; }
        public string SubVehicleNumber { get; set; }
    }

    public class Helper
    {
        private readonly Timer _timer;

        public Helper()
        {
            //timer 10s, mỗi 10s sẽ tự động reset
            var interval = Properties.Settings.Default.Interval;

            if (interval < 0 || interval == null)
            {
                interval = 90;
            }

            _timer = new Timer(1000 * interval) { AutoReset = true };
            _timer.Elapsed += (sender, e) =>
            {
                #region sync from pmc to web

                try
                {
                    ConnectionFactory sqlConnection = new ConnectionFactory();
                    using (var conn = sqlConnection.GetConnection())
                    {
                        using (var context = new Web_BookingTransEntities())
                        {
                            using (var trans = context.Database.BeginTransaction())
                            {
                                //lần chạy cuối cùng
                                var LastUpdateTime = SyncTime.GetInstance().GetLastRun(conn);
                                WriteLogError("Last time run: " + LastUpdateTime.LastRun);
                                WriteLogError("Name: " + LastUpdateTime.Name);
                                //lấy những thông tin sau lần chạy cuối cùng
                                var listVehicleReg = VehicleRegisterMobileDAO.GetInstance().GetVehicleRegisList(conn, LastUpdateTime.LastRun);
                                WriteLogError("Number of Row: " + listVehicleReg.Count);
                                if (listVehicleReg.Count > 0)
                                {
                                    foreach (var item in listVehicleReg)
                                    {
                                        //thông tin từng line
                                        var ln = VehicleRegisterPODetailDAO.GetInstance().GetVehicleRegisDetailList(conn, item.VehicleRegisterMobileId);
                                        //lấy phiếu cân đã tách tải
                                        var query = "SELECT TOP (100) [t1].[ScaleTicketCode] MainCode," +
                                        "[t1].[ActualWeight] MainWeight," +
                                        "[t1].[TotalReduced] MainReduce," +
                                        "[t1].[VehicleNumber] MainVehicleNumber," +
                                        "[t2].[ScaleTicketCode] SubCode," +
                                        "[t2].[ActualWeight] SubWeight," +
                                        "[t2].[TotalReduced] SubReduce," +
                                        "[t2].[VehicleNumber] SubVehicleNumber," +
                                        "[t1].[InvalidSAPMessage]" +
                                        "FROM [dbo].[ScaleTicketModel] [t1]" +
                                        "INNER JOIN [dbo].[ScaleTicketModel] [t2]" +
                                        "ON [t2].[InvalidSAPMessage] = [t1].[InvalidSAPMessage]" +
                                        "WHERE ([t1].[InvalidSAPMessage] IS NOT NULL)" +
                                        "AND ([t1].[SoftCode] = [t2].[SoftCode])" +
                                        "AND ([t1].[ScaleTicketTypeCode] = [t2].[ScaleTicketTypeCode])" +
                                        "AND ([t1].[ScaleTicketCode] <> [t2].[ScaleTicketCode])" +
                                        "AND ([t1].[ScaleTicketCode] = @ticketCode);";
                                        var subTickets = conn.QueryFirstOrDefault<SubTicket>(query, new { ticketCode = item.ScaleTicketCode });
                                        if (subTickets != null)
                                        {
                                            //tạo thông tin dựa trên sub ticket

                                            #region tạo Mapper

                                            var config = new MapperConfiguration(c =>
                                                {
                                                    c.CreateMap<VehicleRegisterMobileModel, VehicleRegisterMobileModel>();
                                                    c.CreateMap<VehicleRegisterPODetailModel, VehicleRegisterPODetailModel>();
                                                }
                                            );
                                            var mapper = new Mapper(config);

                                            #endregion tạo Mapper

                                            #region Thông tin tách

                                            //chỉnh sửa thông tin phiếu tách
                                            var _item = mapper.Map<VehicleRegisterMobileModel>(item);
                                            _item.VehicleRegisterMobileId = Guid.NewGuid();
                                            _item.UserRegisterId = item.UserRegisterId;
                                            _item.TrongLuongGiaoThucTe = subTickets.SubWeight;
                                            _item.TapChat = subTickets.SubReduce;
                                            _item.ScaleTicketCode = subTickets.MainCode + ", " + subTickets.SubCode;
                                            _item.VehicleNumber = subTickets.SubVehicleNumber;
                                            _item.AllowEdit = false;
                                            _item.IsActive = false;
                                            //thêm phiếu tách
                                            context.VehicleRegisterMobileModels.Add(_item);
                                            context.SaveChanges();
                                            //detail
                                            var _ln = new List<VehicleRegisterPODetailModel>();
                                            for (int n = 0; n < ln.Count; n++)
                                            {
                                                var i = mapper.Map<VehicleRegisterPODetailModel>(ln[n]);
                                                if (i != null)
                                                {
                                                    //map lại dữ liệu
                                                    i.VehicleRegisterPODetailId = Guid.NewGuid();
                                                    i.VehicleRegisterMobileId = _item.VehicleRegisterMobileId;
                                                    i.TiLe = 0;
                                                    i.TrongLuong = 0;
                                                    _ln.Add(i);
                                                }
                                            }
                                            if (_ln.Count > 0)
                                            {
                                                context.VehicleRegisterPODetailModels.AddRange(_ln);
                                                context.SaveChanges();
                                            }

                                            #endregion Thông tin tách

                                            //
                                            item.TrongLuongGiaoThucTe = subTickets.MainWeight;
                                            item.TapChat = subTickets.MainReduce;
                                            item.ScaleTicketCode = subTickets.MainCode + ", " + subTickets.SubCode;
                                            //detail
                                        }
                                        //thông tin đăng ký
                                        var regID = context.VehicleRegisterMobileModels
                                        .Where(i => i.VehicleRegisterMobileId == item.VehicleRegisterMobileId)
                                        .Select(i => i.UserRegisterId).FirstOrDefault();
                                        if (regID != null && regID != Guid.Empty)
                                        {
                                            item.UserRegisterId = regID;
                                        }
                                        if (item.ThoiGianToiThucTe == DateTime.MinValue || item.ThoiGianToiThucTe == null)
                                        {
                                            item.ThoiGianToiThucTe = (DateTime?)SqlDateTime.MinValue;
                                        }
                                        context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                                        context.SaveChanges();
                                        //chi tiet
                                        foreach (var line in ln)
                                        {
                                            context.Entry(line).State = System.Data.Entity.EntityState.Modified;
                                            context.SaveChanges();
                                        }
                                    }
                                    context.SaveChanges();
                                    trans.Commit();
                                    SyncTime.GetInstance().UpdateLastRun(conn, LastUpdateTime);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteLogError(ex.Message);
                    throw;
                }

                #endregion sync from pmc to web
            };
        }

        public void Start()
        {
            WriteLogError("Service Start");
            _timer.Start();
        }

        public void Stop()
        {
            WriteLogError("Service Stop");
            _timer.Stop();
        }

        // Ghi lại Log File ở các bước thực thi Service mỗi 60s
        public static void WriteLogError(string message)
        {
            StreamWriter sw = null;
            try
            {
                deleteLog(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt");
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString("g") + ": " + message);
                sw.Flush();
                sw.Close();
            }
            catch
            {
                // ignored
            }
        }

        //xóa log mỗi khi quá nhiều
        private static void deleteLog(string file_path)
        {
            try
            {
                if (new FileInfo(file_path).Length > Int32.MaxValue)
                {
                    File.WriteAllText(file_path, string.Empty);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}