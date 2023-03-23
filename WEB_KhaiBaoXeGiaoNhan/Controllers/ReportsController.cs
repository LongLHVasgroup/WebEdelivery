using AdminPortal.DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WEB_KhaiBaoXeGiaoNhan.Constants;

namespace WEB_KhaiBaoXeGiaoNhan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : BaseController
    {
        [HttpGet("reportnhapxuat")]
        public SingleResponeMessage<DataSet> BaoCaoNhapXuat(DateTime from, DateTime to, string plant)
        {
            var ret = new SingleResponeMessage<DataSet>();

            var cnt = "";
            if (plant == Constants.PlantConstants.P3000)
            {
                cnt = Config.getInstance().connPMC3000;
            }
            else if (plant == Constants.PlantConstants.P4000)
            {
                cnt = Config.getInstance().connPMC;
            }
            else if (plant == Constants.PlantConstants.P6000)
            {
                cnt = Config.getInstance().connPMC6000;
            }
            else
            {
                ret.isSuccess = false;
                ret.item = null;
                return ret;
            }
            try
            {


                using (SqlConnection connection = new SqlConnection(cnt))
                {
                    SqlDataAdapter da = new SqlDataAdapter("Web_BaoCaoNhapXuat", connection);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    if (from == null || from == DateTime.MinValue)
                    {
                        from = DateTime.Today;
                    }
                    if (to == null || to == DateTime.MinValue)
                    {
                        to = DateTime.Today.AddDays(1);
                    }
                    if (from == to)
                    {
                        to = to.AddDays(1);
                    }
                    da.SelectCommand.Parameters.Add(new SqlParameter("@from", from));
                    da.SelectCommand.Parameters.Add(new SqlParameter("@to", to));

                    // DataTable dt = new DataTable();
                    // da.Fill(dt);

                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    ds.Tables[0].TableName = "Data";


                    DataTable dt = new DataTable();
                    dt.Columns.Add("PlantCode");
                    DataRow newRow = dt.NewRow();
                    newRow[0] = plant;
                    dt.Rows.Add(newRow);
                    ds.Tables.Add(dt);
                    ds.Tables[1].TableName = "Plant";


                    ret.isSuccess = true;
                    ret.item = ds;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                ret.isSuccess = false;
                ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Không lấy được thông tin" };
                ret.item = null;
            }
            return ret;
        }

        [HttpGet("reportvanchuyen")]
        public SingleResponeMessage<DataTable> BaoCaoVanChuyen(DateTime from, DateTime to, string plant)
        {
            var ret = new SingleResponeMessage<DataTable>();

            var cnt = "";
            if (plant == Constants.PlantConstants.P3000)
            {
                cnt = Config.getInstance().connPMC3000;
            }
            else if (plant == Constants.PlantConstants.P4000)
            {
                cnt = Config.getInstance().connPMC;
            }
            else if (plant == Constants.PlantConstants.P6000)
            {
                cnt = Config.getInstance().connPMC6000;
            }
            else
            {
                ret.isSuccess = false;
                ret.item = null;
                return ret;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(cnt))
                {
                    SqlDataAdapter da = new SqlDataAdapter("Web_BaoCaoVanChuyen", connection);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    if (from == null || from == DateTime.MinValue)
                    {
                        from = DateTime.Today;
                    }
                    if (to == null || to == DateTime.MinValue)
                    {
                        to = DateTime.Today.AddDays(1);
                    }
                    if (from == to)
                    {
                        to = to.AddDays(1);
                    }
                    da.SelectCommand.Parameters.Add(new SqlParameter("@from", from));
                    da.SelectCommand.Parameters.Add(new SqlParameter("@to", to));

                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ret.isSuccess = true;
                    ret.item = dt;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                ret.isSuccess = false;
                ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Không lấy được thông tin" };
                ret.item = null;
            }
            return ret;
        }

        [HttpGet("reportnhapkhaunoidia")]
        public SingleResponeMessage<DataSet> BaoCaoNhapKhauNoiDia(DateTime from, DateTime to, string plant)
        {
            var ret = new SingleResponeMessage<DataSet>();
            string cnt = "";
            switch (plant)
            {
                case "3000":
                    cnt = Config.getInstance().connPMC3000;
                    break;
                case "4000":
                    cnt = Config.getInstance().connPMC;
                    break;
                case "6000":
                    cnt = Config.getInstance().connPMC6000;
                    break;
                default:
                    ret.isSuccess = false;
                    ret.item = null;
                    return ret;
            }
            using (SqlConnection connection = new SqlConnection(cnt))
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("Web_BaoCaoNhapKhauNoiDia", connection);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    if (from == null || from == DateTime.MinValue)
                    {
                        from = DateTime.Today;
                    }
                    if (to == null || to == DateTime.MinValue)
                    {
                        to = DateTime.Today.AddDays(1);
                    }
                    if (from == to)
                    {
                        to = to.AddDays(1);
                    }
                    da.SelectCommand.Parameters.Add(new SqlParameter("@from", from));
                    da.SelectCommand.Parameters.Add(new SqlParameter("@to", to));

                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    ds.Tables[0].TableName = "NhapKhau";
                    ds.Tables[1].TableName = "NoiDia";
                    ds.Tables[2].TableName = "TongCong";

                    DataTable dt = new DataTable();
                    dt.Columns.Add("PlantCode");
                    DataRow newRow = dt.NewRow();
                    newRow[0] = plant;
                    dt.Rows.Add(newRow);
                    ds.Tables.Add(dt);
                    ds.Tables[3].TableName = "Plant";

                    ret.isSuccess = true;
                    ret.item = ds;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    ret.isSuccess = false;
                    ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Không lấy được thông tin" };
                    ret.item = null;
                }
            }
            return ret;
        }

        [HttpGet("reporttiendogiaohang")]
        public SingleResponeMessage<DataSet> BaoCaoTienDoGiaoHang(string providerCode, string providerName, string poNumber, DateTime from, DateTime to)
        {
            var ret = new SingleResponeMessage<DataSet>();
            using (SqlConnection connection = new SqlConnection(Config.getInstance().connPMC))
            {
                SqlDataAdapter da = new SqlDataAdapter("Web_BaoCaoTienDo", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                if (string.IsNullOrEmpty(providerCode))
                {
                    providerCode = string.Empty;
                }
                if (string.IsNullOrEmpty(providerName))
                {
                    providerName = string.Empty;
                }
                if (string.IsNullOrEmpty(poNumber))
                {
                    poNumber = string.Empty;
                }
                if (from == null || from == DateTime.MinValue)
                {
                    from = DateTime.Today;
                }
                if (to == null || to == DateTime.MinValue)
                {
                    to = DateTime.Today.AddDays(1);
                }
                if (from == to)
                {
                    to = to.AddDays(1);
                }
                da.SelectCommand.Parameters.Add(new SqlParameter("@providerCode", providerCode));
                da.SelectCommand.Parameters.Add(new SqlParameter("@providerName", providerName));
                da.SelectCommand.Parameters.Add(new SqlParameter("@poNumber", poNumber));
                da.SelectCommand.Parameters.Add(new SqlParameter("@from", from));
                da.SelectCommand.Parameters.Add(new SqlParameter("@to", to));
                DataSet ds = new DataSet();
                da.Fill(ds);
                ds.Tables[0].TableName = "TienDoGiaoHang";
                ret.isSuccess = true;
                ret.item = ds;
            }
            return ret;
        }

        [HttpGet("reporttonghop")]
        public SingleResponeMessage<DataTable> BaoCaoTongHop(string ponumber, DateTime from, DateTime to, string plant)
        {
            var ret = new SingleResponeMessage<DataTable>();

            var cnt = "";
            if (plant == Constants.PlantConstants.P3000)
            {
                cnt = Config.getInstance().connPMC3000;
            }
            else if (plant == Constants.PlantConstants.P4000)
            {
                cnt = Config.getInstance().connPMC;
            }
            else if (plant == Constants.PlantConstants.P6000)
            {
                cnt = Config.getInstance().connPMC6000;
            }
            else
            {
                ret.isSuccess = false;
                ret.item = null;
                ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Không tìm thấy thông tin đơn vị" };
                return ret;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(cnt))
                {
                    SqlDataAdapter da = new SqlDataAdapter("Web_BaoCaoTongHop", connection);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    if (string.IsNullOrEmpty(ponumber))
                    {
                        ponumber = string.Empty;
                    }
                    if (from == null || from == DateTime.MinValue)
                    {
                        from = DateTime.Today;
                    }
                    if (to == null || to == DateTime.MinValue)
                    {
                        to = DateTime.Today.AddDays(1);
                    }
                    if (from == to)
                    {
                        to = to.AddDays(1);
                    }
                    da.SelectCommand.Parameters.Add(new SqlParameter("@ponumber", ponumber));
                    da.SelectCommand.Parameters.Add(new SqlParameter("@from", from));
                    da.SelectCommand.Parameters.Add(new SqlParameter("@to", to));

                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ret.isSuccess = true;
                    ret.item = dt;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                ret.isSuccess = false;
                ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Không lấy được thông tin" };
                ret.item = null;
            }


            return ret;
        }

        [HttpGet("reporttinhhinhvanchuyen")]
        public SingleResponeMessage<DataTable> BaoCaoTinhHinhVanChuyen(DateTime from, string plant)
        {
            var ret = new SingleResponeMessage<DataTable>();
            using (SqlConnection connection = new SqlConnection(Config.getInstance().connWeb))
            {

                // Store procedure

                /*

                                        -- =============================================
                                        --Author:		< Hoàng >
                                        --Create date: < 26 / 3 / 2021 >
                                        --Description:	< Báo cáo tình hình vận chuyển của dịch vụ vận chuyển>
                                        -- [Web_BaoCaoVanChuyenTheoDVVC] '2021-03-31'
                                        -- =============================================
                                        CREATE PROCEDURE[dbo].[Web_BaoCaoVanChuyenTheoDVVC]
                                        -- Add the parameters for the stored procedure here
                                        @from DATETIME = '2021-01-01',
                                        @plant NVARCHAR(50)
                                        AS
                                        BEGIN
                                            -- SET NOCOUNT ON added to prevent extra result sets from
                                            ---- interfering with SELECT statements.

                                            SET NOCOUNT ON;

                                        --Insert statements for procedure here

                                           SELECT mapping.OrderNumber,

                                                  [provider].[ProviderCode],

                                                  [provider].[ProviderName],

                                                  [dvvc].ProviderCode DVVCCode,

                                                  [dvvc].ProviderName DVVC,

                                                  [mapping].[SoLuong],

                                                  [mapping].[SoLuongCont],
                                                  (ISNULL(SUM(reg.TrongLuongGiaoThucTe), 0)) AS[DaVanChuyen],
                                                  (COUNT(reg.ScaleTicketCode)) AS NumberOfTrans,
                                                  (
                                                  (
                                                      SELECT ISNULL(
                                                             (
                                                                 SELECT(ISNULL(SUM(reg.TrongLuongGiaoThucTe), 0))
                                                                        / NULLIF(
                                                                          (
                                                                              SELECT COUNT(reg.ScaleTicketCode)
                                                                          ), 0)
                                                             ),
                                                             0
                                                                   )
                                       )
                                       ) AS[TrongLuongTrungBinh],
                                       --((ISNULL(SUM(reg.TrongLuongGiaoThucTe), 0)) / [mapping].[SoLuong]) AS[TyLeHoanThanh],
                                       [cungduong].[CungDuongCode],
                                       [cungduong].[CungDuongName]
                                FROM[OrderMapping][mapping]
                                    LEFT JOIN[ProviderModel] [dvvc] WITH(NOLOCK)
                                        ON[mapping].[ServiceID] = [dvvc].[ProviderId]
                                    LEFT JOIN ProviderModel[provider] WITH(NOLOCK)
                                        ON mapping.MasterID = provider.ProviderId
                                    INNER JOIN POMasterModel[pom]

                                        ON mapping.OrderNumber = pom.PONumber AND pom.CompanyCode = @plant
                                    LEFT JOIN dbo.VehicleRegisterMobileModel reg WITH(NOLOCK)
                                        ON reg.SoDonHang = mapping.OrderNumber
                                    LEFT JOIN CungDuongModel cungduong WITH(NOLOCK)
                                        ON cungduong.CungDuongCode = mapping.CungDuongCode
                                WHERE
                                    [reg].[RegisterTime] >= @from
                                GROUP BY mapping.OrderNumber,
                                         [provider].[ProviderCode],
                                         [provider].[ProviderName],
                                         [dvvc].[ProviderCode],
                                         [dvvc].[ProviderName],
                                         [mapping].[SoLuong],
                                         [mapping].[SoLuongCont],
                                         [cungduong].[CungDuongCode],
                                         [cungduong].[CungDuongName];
                                            END;

                */


                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("Web_BaoCaoVanChuyenTheoDVVC", connection);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;

                    if (from == null || from == DateTime.MinValue)
                    {
                        from = DateTime.Today;
                    }
                    if (plant == null)
                    {
                        plant = "";
                    }

                    da.SelectCommand.Parameters.Add(new SqlParameter("@from", from));
                    da.SelectCommand.Parameters.Add(new SqlParameter("@plant", plant));
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ret.isSuccess = true;
                    ret.item = dt;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    ret.isSuccess = false;
                    ret.item = null;
                    ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Không lấy được thông tin" };
                }

            }
            return ret;
        }

        [HttpGet("lichsudieuphoi")]
        public SingleResponeMessage<DataTable> LichSuDieuPhoi(DateTime from, DateTime to)
        {
            // -- =============================================
            // -- Author:		<Phat>
            // -- Create date: <19/06/2021>
            // -- Description:	<Báo cáo danh sách đơn đã điều phối>
            // -- =============================================
            // CREATE PROCEDURE [dbo].[Web_LichSuDieuPhoi]
            // -- Add the parameters for the stored procedure here
            // @from DATETIME ,
            // @to DATETIME ,
            // @username nvarchar(50)
            // AS
            // BEGIN
            //     -- SET NOCOUNT ON added to prevent extra result sets from
            //     ---- interfering with SELECT statements.
            //     SET NOCOUNT ON;

            //     -- Insert statements for procedure here
            //     SELECT A.MappingID, A.OrderNumber, A.SoLuong, A.SoLuongCont, A.BillNumber, A.ShipNumber, A.DVVC, A.CreatedTime, B.ProviderName as NCC, C.CungDuongName
            // 	from (select OrderMapping.*, ProviderModel.ProviderName as DVVC from OrderMapping
            // 		left join ProviderModel
            // 		on OrderMapping.ServiceID =  ProviderModel.ProviderId
            // 		where OrderMapping.CreatedTime <= @to AND OrderMapping.CreatedTime >= @from)  as A
            // 		left join ProviderModel as B
            // 		on A.MasterID =  B.ProviderId
            // 		left join CungDuongModel as C
            // 		on A.CungDuongCode = C.CungDuongCode
            // 		INNER join (select PONumber, POMasterModel.CompanyCode as CompanyCode from POMasterModel, UserModel where POMasterModel.CompanyCode = UserModel.CompanyCode AND UserModel.username = @username) as ByUser
            // 		on A.Ordernumber = ByUser.PONumber AND C.Plant = ByUser.CompanyCode

            // order by CreatedTime, A.OrderNumber desc
            // END;

            var ret = new SingleResponeMessage<DataTable>();
            var username = GetUserId();
            using (SqlConnection connection = new SqlConnection(Config.getInstance().connWeb))
            {
                SqlDataAdapter da = new SqlDataAdapter("Web_LichSuDieuPhoi", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;

                if (from == null)
                    from = DateTime.Today;
                if (to == null)
                    to = DateTime.Today;
                to = to.AddDays(1);
                da.SelectCommand.Parameters.Add(new SqlParameter("@from", from));
                da.SelectCommand.Parameters.Add(new SqlParameter("@to", to));
                da.SelectCommand.Parameters.Add(new SqlParameter("@username", username));
                DataTable dt = new DataTable();
                da.Fill(dt);
                ret.isSuccess = true;
                ret.item = dt;
            }
            return ret;
        }

        [HttpGet("tinhhinhdangky")]
        public SingleResponeMessage<DataTable> TinhHinhDangKy(DateTime from, DateTime to, string plant)
        {

            var ret = new SingleResponeMessage<DataTable>();
            using (SqlConnection connection = new SqlConnection(Config.getInstance().connWeb))
            {
                SqlDataAdapter da = new SqlDataAdapter("Web_Report_DanhSachDangKy", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;

                if (from == null)
                    from = DateTime.Today;
                if (to == null)
                    to = DateTime.Today;
                if (String.IsNullOrEmpty(plant))
                {
                    ret.isSuccess = false;
                    ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Không xác định được plant code" };
                    ret.item = null;
                    return ret;
                }
                to = to.AddDays(1);
                da.SelectCommand.Parameters.Add(new SqlParameter("@from", from));
                da.SelectCommand.Parameters.Add(new SqlParameter("@to", to));
                da.SelectCommand.Parameters.Add(new SqlParameter("@plant", plant));
                DataTable dt = new DataTable();
                da.Fill(dt);
                ret.isSuccess = true;
                ret.item = dt;
            }
            return ret;
        }

        [HttpGet("Web_Chart_Summary")]
        public SingleResponeMessage<DataSet> Web_Chart_Summary(DateTime from, DateTime to, string plant)
        {
            var ret = new SingleResponeMessage<DataSet>();
            var cnt = "";
            switch (plant)
            {
                case "3000":
                    cnt = Config.getInstance().connPMC3000;
                    break;
                case "4000":
                    cnt = Config.getInstance().connPMC;
                    break;
                case "6000":
                    cnt = Config.getInstance().connPMC6000;
                    break;
                default:
                    ret.isSuccess = false;
                    ret.item = null;
                    ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Không lấy được thông tin" };
                    return ret;

            }

            try
            {


                using (SqlConnection connection = new SqlConnection(cnt))
                {
                    SqlDataAdapter da = new SqlDataAdapter("Web_Chart_Summary", connection);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    if (from == null || from == DateTime.MinValue)
                    {
                        from = DateTime.Today;
                    }
                    if (to == null || to == DateTime.MinValue)
                    {
                        to = DateTime.Today.AddDays(1);
                    }
                    if (from == to)
                    {
                        to = to.AddDays(1);
                    }
                    if (to > from)
                    {
                        to = to.AddDays(1);
                    }
                    da.SelectCommand.Parameters.Add(new SqlParameter("@from", from));
                    da.SelectCommand.Parameters.Add(new SqlParameter("@to", to));

                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    ds.Tables[0].TableName = "ByDate";
                    ds.Tables[1].TableName = "ByProvider";

                    // Thêm plant code trả về
                    DataTable dt = new DataTable();
                    dt.Columns.Add("PlantCode");
                    DataRow newRow = dt.NewRow();
                    newRow[0] = plant;
                    dt.Rows.Add(newRow);
                    ds.Tables.Add(dt);
                    ds.Tables[2].TableName = "Plant";


                    ret.isSuccess = true;
                    ret.item = ds;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                ret.isSuccess = false;
                ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Không lấy được thông tin" };
                ret.item = null;
            }
            return ret;
        }
    }
}