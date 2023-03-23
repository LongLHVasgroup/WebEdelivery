using ApiTest.Common;
using AutoMapper;
using Common;
using Dapper;
using Service.ADO;
using Service.Properties;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Service
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //khởi tạo connection string
            ConnectionString connection = ConnectionString.getInstance();
            connection.connectionString = Settings.Default.ConnectionString;

            #region Service

            //khởi tạo service
            //chạy liên tục tới khi nào app tắt
            var exitCode = HostFactory.Run(sc =>
            {
                sc.Service<Helper>(s =>
                {
                    s.ConstructUsing(helper => new Helper());
                    s.WhenStarted(helper => helper.Start());
                    s.WhenStopped(helper => helper.Stop());
                });
                sc.RunAsLocalSystem();
                sc.SetServiceName("SyncPmcToWeb");
                sc.SetDisplayName("SyncPmcToWeb");
                sc.SetDescription("Đồng bộ phần mềm cân tới web bán hàng");
            });
            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;

            #endregion Service

            #region test

            //Test();

            #endregion test
        }

        private static void Test()
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
                            //WriteLogError("Last time run: " + LastUpdateTime.LastRun);
                            //WriteLogError("Name: " + LastUpdateTime.Name);
                            //lấy những thông tin sau lần chạy cuối cùng
                            var listVehicleReg = VehicleRegisterMobileDAO.GetInstance().GetVehicleRegisList(conn, LastUpdateTime.LastRun);
                            //WriteLogError("Number of Row: " + listVehicleReg.Count);
                            if (listVehicleReg.Count > 0)
                            {
                                foreach (var item in listVehicleReg)
                                {
                                    //thông tin từng line
                                    var ln = VehicleRegisterPODetailDAO.GetInstance().GetVehicleRegisDetailList(conn, item.VehicleRegisterMobileId);
                                    //lấy phiếu cân đã tách tải
                                    var query = "SELECT TOP (100) [t1].[ScaleTicketCode] MainCode,[t1].[ActualWeight] MainWeight,[t1].[TotalReduced] MainReduce,[t1].[VehicleNumber] MainVehicleNumber,[t2].[ScaleTicketCode] SubCode,[t2].[ActualWeight] SubWeight,[t2].[TotalReduced] SubReduce,[t2].[VehicleNumber] SubVehicleNumber,[t1].[InvalidSAPMessage]FROM [dbo].[ScaleTicketModel] [t1]INNER JOIN [dbo].[ScaleTicketModel] [t2]ON [t2].[InvalidSAPMessage] = [t1].[InvalidSAPMessage]WHERE ([t1].[InvalidSAPMessage] IS NOT NULL)AND ([t1].[SoftCode] = [t2].[SoftCode])AND ([t1].[ScaleTicketTypeCode] = [t2].[ScaleTicketTypeCode])AND ([t1].[ScaleTicketCode] <> [t2].[ScaleTicketCode])AND ([t1].[ScaleTicketCode] = @ticketCode);";
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
                //WriteLogError(ex.Message);
                throw;
            }

            #endregion sync from pmc to web
        }
    }
}