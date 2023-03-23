using Dapper;
using SAP.Middleware.Connector;
using ServiceSyncPoToWeb.Models;
using ServiceSyncPoToWeb.repositories;
using ServiceSyncPoToWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;


namespace ServiceSyncPoToWeb
{
    public partial class SyncToWeb : ServiceBase
    {

        //Initialize the timer
        private Timer timer = new Timer();

        private static string serviceName = "eDelivery SyncFromPMC";

        //private static string serviceDescription = "BIS is service which is used to transfer data";
        private static string appPath = System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);
        private static string logPath = Path.GetFullPath(appPath + @"\logs");



        //private static string executableServicePath = System.IO.Path.Combine(appPath, "WinServiceSyncToSAP.exe");
        private string logFilePath = System.IO.Path.Combine(appPath, "eDelivery-SyncFromPMC-" + DateTime.Now.ToString("yyyyMMdd") + ".log");

        private static string cnt_Web = ConfigurationManager.ConnectionStrings["eDeliveryConnectionString"].ConnectionString;
        private static string cnt_P3 = ConfigurationManager.ConnectionStrings["3000ConnectionString"].ConnectionString;
        private static string cnt_P4 = ConfigurationManager.ConnectionStrings["4000ConnectionString"].ConnectionString;
        private static string cnt_P6 = ConfigurationManager.ConnectionStrings["6000ConnectionString"].ConnectionString;
        private static string plant3 = ConfigurationManager.AppSettings["PLANT_3000"].ToString();
        private static string plant4 = ConfigurationManager.AppSettings["PLANT_4000"].ToString();
        private static string plant6 = ConfigurationManager.AppSettings["PLANT_6000"].ToString();

        private static int Interval = int.Parse(ConfigurationManager.AppSettings["Interval"].ToString());


        public SyncToWeb()
        {
            InitializeComponent();
            this.ServiceName = serviceName;
            this.CanHandlePowerEvent = true;
            this.CanHandleSessionChangeEvent = true;
            this.CanPauseAndContinue = true;
            this.CanShutdown = true;
            this.CanStop = true;
            this.AutoLog = false; //if use custom service


            #region đồng bộ sang SAP

            //handle Elapsed event
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            //15 phút chạy 1 lần
            //timer.Interval = 1000 * 60 * 15;
            //30 giây chạy 1 lần
            //timer.Interval = 1000 * 30;
            timer.Interval = Interval;

            timer.Enabled = true;

            #endregion đồng bộ sang SAP

        }

        private void OnElapsedTime(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }
                logFilePath = System.IO.Path.Combine(logPath, "eDelivery-SyncFromPMC-" + DateTime.Now.ToString("yyyyMMdd") + ".log");
            }
            catch (Exception)
            {
                logFilePath = System.IO.Path.Combine(appPath, "eDelivery-SyncFromPMC-" + DateTime.Now.ToString("yyyyMMdd") + ".log");
            }
            

            WriteLogFile(logFilePath, "Service start at OnElapsedTime");

            #region Service start

            //SyncMasterFromSAPToPMC();

            //Đồng bộ thoong tin master
            PullMasterData();

            // Dong Bo thong tin PO
            PullPOData();

            #endregion Service start
        }

        private void PullPOData()
        {


            //PullPoFromPMC(cnt_P1, plant1);

            //PullPoFromPMC(cnt_P3, plant3);

            PullPoFromPMC(cnt_P3, plant3);

            PullPoFromPMC(cnt_P4, plant4);

            PullPoFromPMC(cnt_P6, plant6);

        }

        private void PullPoFromPMC(string cnt, string plant)
        {
            try
            {
                WriteLogFile(logFilePath, "SyncPOFromPMC START PLANT " + plant);
                int update = 0;
                int insert = 0;
                using (var connectionPMC = new SqlConnection(cnt))
                {
                    using (var connectionWEB = new SqlConnection(cnt_Web))
                    {
                        var prQuery = new DynamicParameters();
                        prQuery.Add("@DeliveryDate", DateTime.Now.AddDays(-2));
                        var lstPoLine = connectionPMC.Query<POLineModel>(@"select POLineModel.* from dbo.POLineModel, dbo.POMasterModel WITH (NOLOCK) Where dbo.POLineModel.PONumber  = dbo.POMasterModel.PONumber and DeliveryDate >= @DeliveryDate  order by PONumber desc", prQuery).ToList();
                        for (int i = 0; i < lstPoLine.Count(); i++)
                        {
                            // Check to update
                            // WriteLogFile(logFilePath, lstPoLine[i].PONumber.ToString());
                            var prPoLine = new DynamicParameters();
                            prPoLine.Add("@PONumber", lstPoLine[i].PONumber);
                            var hasExists = connectionWEB.Query<int>(@"select 1 from dbo.POMasterModel WITH (NOLOCK) where PONumber=@PONumber", prPoLine).FirstOrDefault();
                            try
                            {
                                if (hasExists > 0)
                                {
                                    #region sync Update
                                    var pItemRowPoline = new DynamicParameters();
                                    pItemRowPoline.Add("PONumber", lstPoLine[i].PONumber);
                                    pItemRowPoline.Add("POLine", lstPoLine[i].POLine);
                                    pItemRowPoline.Add("ProductCode", lstPoLine[i].ProductCode);
                                    pItemRowPoline.Add("ProductName", lstPoLine[i].ProductName);
                                    pItemRowPoline.Add("Qty", lstPoLine[i].Qty);
                                    pItemRowPoline.Add("UNIT", lstPoLine[i].Unit);
                                    pItemRowPoline.Add("OverTolerance", lstPoLine[i].OverTolerance);
                                    pItemRowPoline.Add("UnderTolerance", lstPoLine[i].UnderTolerance);
                                    pItemRowPoline.Add("isUnlimited", lstPoLine[i].IsUnlimited);
                                    pItemRowPoline.Add("DocumentDate", lstPoLine[i].DocumentDate);
                                    pItemRowPoline.Add("DeliveryDate", lstPoLine[i].DeliveryDate);
                                    pItemRowPoline.Add("isRelease", lstPoLine[i].IsRelease);
                                    pItemRowPoline.Add("isDeliveryCompleted", lstPoLine[i].IsDeliveryCompleted);
                                    pItemRowPoline.Add("WarehouseCode", lstPoLine[i].WarehouseCode);
                                    try
                                    {

                                        //Update POMaster
                                        connectionWEB.Execute(@"update POLineModel set
                                                            ProductCode =@ProductCode ,
                                                            ProductName =@ProductName ,
                                                            Qty =@Qty,
                                                            UNIT=@UNIT ,
                                                            OverTolerance = @OverTolerance,
                                                            UnderTolerance= @UnderTolerance ,
                                                            isUnlimited = @isUnlimited,
                                                            DocumentDate= @DocumentDate ,
                                                            DeliveryDate = @DeliveryDate,
                                                            isRelease= @isRelease ,
                                                            isDeliveryCompleted= @isDeliveryCompleted,
                                                            WarehouseCode= @WarehouseCode 
                                                    where POLine = @POLine and PONumber=@PONumber", pItemRowPoline);


                                        if (i == lstPoLine.Count() - 1)
                                        {
                                            var PoMasterData = connectionPMC.Query<POMasterModel>(@"select TOP (1) * from dbo.POMasterModel WITH (NOLOCK) where PONumber=@PONumber", prPoLine).FirstOrDefault();

                                            var pPOMaster = new DynamicParameters();
                                            pPOMaster.Add("PONumber", PoMasterData.PONumber);
                                            pPOMaster.Add("ProviderCode", PoMasterData.ProviderCode);
                                            pPOMaster.Add("ProviderName", PoMasterData.ProviderName);
                                            pPOMaster.Add("QtyTotal", PoMasterData.QtyTotal);
                                            pPOMaster.Add("isNhapKhau", PoMasterData.IsNhapKhau);
                                            pPOMaster.Add("isCompelete", PoMasterData.IsCompelete);
                                            pPOMaster.Add("Note", PoMasterData.Note);
                                            pPOMaster.Add("SoLuongDaNhap", PoMasterData.SoLuongDaNhap);
                                            pPOMaster.Add("CompanyCode", plant);

                                            connectionWEB.Execute(@"update POMasterModel set ProviderCode = @ProviderCode, ProviderName = @ProviderName, QtyTotal=@QtyTotal, isNhapKhau = @isNhapKhau, isCompelete = @isCompelete, Note=@Note, SoLuongDaNhap = @SoLuongDaNhap, CompanyCode = @CompanyCode where PONumber=@PONumber", pPOMaster);
                                            
                                        }
                                        else
                                        {
                                            if (lstPoLine[i].PONumber != lstPoLine[i + 1].PONumber)
                                            {
                                                var PoMasterData = connectionPMC.Query<POMasterModel>(@"select TOP (1) * from dbo.POMasterModel WITH (NOLOCK) where PONumber=@PONumber", prPoLine).FirstOrDefault();

                                                var pPOMaster = new DynamicParameters();
                                                pPOMaster.Add("PONumber", PoMasterData.PONumber);
                                                pPOMaster.Add("ProviderCode", PoMasterData.ProviderCode);
                                                pPOMaster.Add("ProviderName", PoMasterData.ProviderName);
                                                pPOMaster.Add("QtyTotal", PoMasterData.QtyTotal);
                                                pPOMaster.Add("isNhapKhau", PoMasterData.IsNhapKhau);
                                                pPOMaster.Add("isCompelete", PoMasterData.IsCompelete);
                                                pPOMaster.Add("Note", PoMasterData.Note);
                                                pPOMaster.Add("SoLuongDaNhap", PoMasterData.SoLuongDaNhap);
                                                pPOMaster.Add("CompanyCode", plant);

                                                connectionWEB.Execute(@"update POMasterModel set ProviderCode = @ProviderCode, ProviderName = @ProviderName, QtyTotal=@QtyTotal, isNhapKhau = @isNhapKhau, isCompelete = @isCompelete, Note=@Note, SoLuongDaNhap = @SoLuongDaNhap, CompanyCode = @CompanyCode where PONumber=@PONumber", pPOMaster);
                                                
                                            }
                                        }
                                        update++;

                                    }
                                    catch (Exception ex)
                                    {
                                        WriteLogFile(logFilePath, "ERROR: " + ex.ToString());
                                    }

                                    #endregion sync update
                                }
                                else
                                {
                                    #region sync Add

                                    var pItemRowPoline = new DynamicParameters();
                                    pItemRowPoline.Add("PONumber", lstPoLine[i].PONumber);
                                    pItemRowPoline.Add("POLine", lstPoLine[i].POLine);
                                    pItemRowPoline.Add("ProviderCode", lstPoLine[i].ProviderCode);
                                    pItemRowPoline.Add("ProviderName", lstPoLine[i].ProviderName);
                                    pItemRowPoline.Add("CompanyCode", lstPoLine[i].CompanyCode);
                                    pItemRowPoline.Add("ProductCode", lstPoLine[i].ProductCode);
                                    pItemRowPoline.Add("ProductName", lstPoLine[i].ProductName);
                                    pItemRowPoline.Add("Qty", lstPoLine[i].Qty);
                                    pItemRowPoline.Add("UNIT", lstPoLine[i].Unit);
                                    pItemRowPoline.Add("OverTolerance", lstPoLine[i].OverTolerance);
                                    pItemRowPoline.Add("UnderTolerance", lstPoLine[i].UnderTolerance);
                                    pItemRowPoline.Add("isUnlimited", lstPoLine[i].IsUnlimited);
                                    pItemRowPoline.Add("DocumentDate", lstPoLine[i].DocumentDate);
                                    pItemRowPoline.Add("DeliveryDate", lstPoLine[i].DeliveryDate);
                                    pItemRowPoline.Add("isRelease", lstPoLine[i].IsRelease);
                                    pItemRowPoline.Add("isDeliveryCompleted", lstPoLine[i].IsDeliveryCompleted);
                                    pItemRowPoline.Add("WarehouseCode", lstPoLine[i].WarehouseCode);

                                    connectionWEB.Execute(@"INSERT INTO dbo.POLineModel
                                                    (   POLine ,
                                                        PONumber ,
                                                        ProviderCode,
                                                        ProviderName,
                                                        CompanyCode ,
                                                        ProductCode ,
                                                        ProductName ,
                                                        Qty ,
                                                        UNIT ,
                                                        OverTolerance ,
                                                        UnderTolerance ,
                                                        isUnlimited ,
                                                        DocumentDate ,
                                                        DeliveryDate ,
                                                        isRelease ,
                                                        isDeliveryCompleted ,
                                                        WarehouseCode
                                                    )
                                                values(
                                                        @POLine ,
                                                        @PONumber ,
                                                        @ProviderCode,
                                                        @ProviderName,
                                                        @CompanyCode ,
                                                        @ProductCode ,
                                                        @ProductName ,
                                                        @Qty ,
                                                        @UNIT ,
                                                        @OverTolerance ,
                                                        @UnderTolerance ,
                                                        @isUnlimited ,
                                                        @DocumentDate ,
                                                        @DeliveryDate ,
                                                        @isRelease ,
                                                        @isDeliveryCompleted ,
                                                        @WarehouseCode
                                                        )", pItemRowPoline);


                                    // Add POMaster
                                    //Update POMaster
                                    try
                                    {
                                        if (i == lstPoLine.Count() - 1) // dòng cuối cùng
                                        {
                                            var PoMasterData = connectionPMC.Query<POMasterModel>(@"select TOP (1) * from dbo.POMasterModel WITH (NOLOCK) where PONumber=@PONumber", prPoLine).FirstOrDefault();

                                            var pPOMaster = new DynamicParameters();
                                            pPOMaster.Add("PONumber", PoMasterData.PONumber);
                                            pPOMaster.Add("ProviderCode", PoMasterData.ProviderCode);
                                            pPOMaster.Add("ProviderName", PoMasterData.ProviderName);
                                            pPOMaster.Add("QtyTotal", PoMasterData.QtyTotal);
                                            pPOMaster.Add("isNhapKhau", PoMasterData.IsNhapKhau);
                                            pPOMaster.Add("isCompelete", PoMasterData.IsCompelete);
                                            pPOMaster.Add("Note", PoMasterData.Note);
                                            pPOMaster.Add("SoLuongDaNhap", PoMasterData.SoLuongDaNhap);
                                            pPOMaster.Add("CompanyCode", plant);

                                            connectionWEB.Execute(@" INSERT INTO dbo.POMasterModel
                                                                ( PONumber ,
                                                                ProviderCode ,
                                                                ProviderName ,
                                                                QtyTotal ,
                                                                isNhapKhau ,
                                                                isCompelete,
                                                                Note,
                                                                SoLuongDaNhap,
                                                                CompanyCode
                                                                )
                                                                values(
                                                                        @PONumber ,
                                                                        @ProviderCode,
                                                                        @ProviderName,
                                                                        @QtyTotal ,
                                                                        @isNhapKhau ,
                                                                        @isCompelete ,
                                                                        @Note ,
                                                                        @SoLuongDaNhap ,
                                                                        @CompanyCode 
                                                                        )", pPOMaster);
                                        }
                                        else
                                        {
                                            if (lstPoLine[i].PONumber != lstPoLine[i + 1].PONumber)
                                            {
                                                var PoMasterData = connectionPMC.Query<POMasterModel>(@"select TOP (1) * from dbo.POMasterModel WITH (NOLOCK) where PONumber=@PONumber", prPoLine).FirstOrDefault();

                                                if (PoMasterData != null)
                                                {
                                                    var pPOMaster = new DynamicParameters();
                                                    pPOMaster.Add("PONumber", PoMasterData.PONumber);
                                                    pPOMaster.Add("ProviderCode", PoMasterData.ProviderCode);
                                                    pPOMaster.Add("ProviderName", PoMasterData.ProviderName);
                                                    pPOMaster.Add("QtyTotal", PoMasterData.QtyTotal);
                                                    pPOMaster.Add("isNhapKhau", PoMasterData.IsNhapKhau);
                                                    pPOMaster.Add("isCompelete", PoMasterData.IsCompelete);
                                                    pPOMaster.Add("Note", PoMasterData.Note);
                                                    pPOMaster.Add("SoLuongDaNhap", PoMasterData.SoLuongDaNhap);
                                                    pPOMaster.Add("CompanyCode", plant);
                                                    
                                                    connectionWEB.Execute(@" INSERT INTO dbo.POMasterModel
                                                                ( PONumber ,
                                                                ProviderCode ,
                                                                ProviderName ,
                                                                QtyTotal ,
                                                                isNhapKhau ,
                                                                isCompelete,
                                                                Note,
                                                                SoLuongDaNhap,
                                                                CompanyCode
                                                                )
                                                                values(
                                                                        @PONumber ,
                                                                        @ProviderCode,
                                                                        @ProviderName,
                                                                        @QtyTotal ,
                                                                        @isNhapKhau ,
                                                                        @isCompelete ,
                                                                        @Note ,
                                                                        @SoLuongDaNhap ,
                                                                        @CompanyCode 
                                                                        )", pPOMaster);
                                                }
                                            }
                                        }
                                        insert++;

                                    }
                                    catch (Exception ex)
                                    {
                                        WriteLogFile(logFilePath, "ERROR: " + ex.ToString());
                                    }

                                    #endregion sync Add
                                }
                            }
                            catch (Exception ex)
                            {
                                WriteLogFile(logFilePath, "ERROR: " + ex.ToString());
                            }
                        }
                    }

                }

                WriteLogFile(logFilePath, "updated: " + update);
                WriteLogFile(logFilePath, "insert: " + insert);
            }
            catch (Exception ex)
            {
                WriteLogFile(logFilePath, "SyncPOFromPMC ERROR:" + ex.ToString());
            }
            return;
        }

        private void PullMasterData()
        {
            //throw new NotImplementedException();
            return;
        }

        private void WriteLogFile(string logFilePath, string message)
        {
            if (System.IO.File.Exists(logFilePath))
            {
                if (!System.IO.File.Exists(logFilePath))
                    System.IO.File.Create(logFilePath);
            }
            using (FileStream fileStream = new FileStream(logFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                fileStream.Flush();
                fileStream.Close();
            }

            using (StreamWriter sw = new StreamWriter(logFilePath, true))
            {
                string lastRecordText = "# " + System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " # " + Environment.NewLine + "#" + message + " #" + Environment.NewLine;
                sw.WriteLine(lastRecordText);
                sw.Close();
            }
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}
