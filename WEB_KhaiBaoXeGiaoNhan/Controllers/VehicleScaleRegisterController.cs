using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WEB_KhaiBaoXeGiaoNhan.Datalayers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WEB_KhaiBaoXeGiaoNhan.Controllers
{
    //public class VehicleScaleRegisterController : ControllerBase
    //{
    //    // GET: /<controller>/
    //    public List<ScaleVehicleRegister> Index()
    //    {
    //        return new List<ScaleVehicleRegister>();
    //    }
    //}
    [ApiController]
    [Route("[controller]")]
    public class VehicleScaleRegisterController : BaseController
    {
        [HttpGet("test")]
        public void Get()
        {
            var lstVehicle = VehicleModelDAO.GetInstance().GetList();
            var lstProvider = ProviderModelDAO.GetInstance().GetList();
            var listServicesCode = lstVehicle
                .Where(v => v.VehicleOwner != null)
                .Where(v => v.VehicleOwner.StartsWith("S"))
                .Select(v => v.VehicleOwner.Remove(0, 1))
                .Distinct()
                .ToList();
            var listService = lstProvider.Where(p => listServicesCode.Contains(p.ProviderCode)).ToList();
        }
        /*
        [HttpGet("report")]
        public DataSet Report()
        {
            DataSet dataset = new DataSet();

            try
            {
                using (SqlConnection com = new SqlConnection("Server = 192.168.100.66; Database = Vas_4000; User Id = sa; Password = 123;"))
                {
                    com.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = com;

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "mps_Report2NhapXuatHang";

                        cmd.Parameters.AddWithValue("@type", 2);
                        cmd.Parameters.AddWithValue("@SoftCode", "");
                        cmd.Parameters.AddWithValue("@DonVi", "Cty TNHH Thép Kim Đại Thắng");

                        cmd.Parameters.AddWithValue("@MaSoDonVi", "1000017862");
                        cmd.Parameters.AddWithValue("@HopDong", "");
                        cmd.Parameters.AddWithValue("@DonHang", "");
                        cmd.Parameters.AddWithValue("@DonViVanChuyen", "");
                        cmd.Parameters.AddWithValue("@MaSoDonViVanChuyen", "");

                        cmd.Parameters.AddWithValue("@MaVatTu", "");
                        cmd.Parameters.AddWithValue("@TickIP", 0);

                        cmd.Parameters.AddWithValue("@FromDate", "2021-04-14");

                        cmd.Parameters.AddWithValue("@ToDate", "2021-04-15");

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dataset);
                            dataset.Tables[0].TableName = "Header";
                            dataset.Tables[1].TableName = "Master";
                            dataset.Tables[2].TableName = "Detail";
                            dataset.Tables[3].TableName = "SumDetailGroup";
                            dataset.Tables[4].TableName = "SumVehicle";
                            dataset.Relations.Add("Master_Detail", dataset.Tables["Master"].Columns["SoDonHang"], dataset.Tables["Detail"].Columns["SoDonHang"]);
                        }
                    }
                    return dataset;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine("" + ee.Message.ToString());
                return dataset;
            }
        }*/
    }
}