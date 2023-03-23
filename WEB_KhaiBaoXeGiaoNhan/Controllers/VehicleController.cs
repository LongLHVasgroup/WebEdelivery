using Microsoft.AspNetCore.Mvc;
using Models.Common;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using WEB_KhaiBaoXeGiaoNhan.Models;
using WEB_KhaiBaoXeGiaoNhan.Services;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : BaseController
    {
        [HttpGet("getlistvehicle")]
        public ListResponeMessage<ResponseVehicle> GetList(int pageSize, int pageNumber, string vehiclenumber)
        {
            var ret = new ListResponeMessage<ResponseVehicle>();
            var username = GetUserId();
            var data = VehicleServices.GetInstance().GetAll(username, null);
            if (data != null)
            {
                ret.isSuccess = true;
                ret.data = data.Where(v => v.VehicleNumber.Replace("-", "").Replace(".", "").Replace(" ", "").ToUpper().Contains(vehiclenumber.Replace("-", "").Replace(".", "").Replace(" ", "").ToUpper()) || vehiclenumber == "all")
                               .Skip(pageSize * (pageNumber - 1))
                               .Take(pageSize).ToList();
                ret.totalRecords = data.Count;
                ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Lấy thông tin xe thành công" };
            }
            else
            {
                ret.isSuccess = false;
                ret.data = null;
                ret.totalRecords = 0;
                ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Lấy thông tin thất bại" };
            }
            return ret;
        }
       
        [HttpGet("find")]
        public ListResponeMessage<ResponseVehicle> GetListByNumber(string vehiclenumber, string type)
        {
            var ret = new ListResponeMessage<ResponseVehicle>();
            var username = GetUserId();
            var data = VehicleServices.GetInstance().GetAll(username, type);
            if (!string.IsNullOrEmpty(vehiclenumber))
            {
                if (data != null)
                {
                    ret.isSuccess = true;
                    ret.data = data.Where(v => v.VehicleNumber.Replace("-", "").Replace(".", "").Replace(" ", "")
                    .ToUpper()
                    .Contains(vehiclenumber.Replace("-", "").Replace(".", "").Replace(" ", "").ToUpper()))
                                          .ToList();
                    ret.totalRecords = data.Count;
                    ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Lấy thông tin xe thành công" };
                }
                else
                {
                    ret.isSuccess = false;
                    ret.data = null;
                    ret.totalRecords = 0;
                    ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Lấy thông tin thất bại" };
                }
            }
            return ret;
        }

        [HttpPost("create")]
        public ActionMessage CreateNew([FromBody] CreateVehicle item)
        {
            var ret = new ActionMessage();

            if (item != null)
            {
                var bsx = item.VehicleNumber;
                // check chuẩn hoá biển số xe
                //if(!Regex.IsMatch(bsx, "[0-9][0-9][A-Z]-[0-9][0-9][0-9][0-9]$")){ // 51C-1234
                //    if(!Regex.IsMatch(bsx, "[0-9][0-9][A-Z]-[0-9][0-9][0-9]\\.[0-9][0-9]$")){ // 51C-123.45
                //        if(!Regex.IsMatch(bsx, "[0-9][0-9][A-Z][A-Z]-[0-9][0-9][0-9][0-9]$")){ // 51LD-1234
                //            if(!Regex.IsMatch(bsx, "[0-9][0-9][A-Z][A-Z]-[0-9][0-9][0-9]\\.[0-9][0-9]$")){ // 51LD-123.45
                //                ret.isSuccess = false;
                //                ret.id = 0;
                //                ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Biển số xe phải có dạng: 51C-1234; 51C-123.45; 51LD-1234; 51LD-123.45" };
                                
                //                return ret;
                //            } 
                //        } 
                //    } 
                //}
                var id = VehicleServices.GetInstance().CreateNew(item, GetUserId());
                if (id > 0)
                {
                    ret.isSuccess = true;
                    ret.id = id;
                    ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Thêm xe thành công" };
                }
                else if( id == -1)
                {
                    ret.isSuccess = false;
                    ret.id = id;
                    ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Biển số xe đã tồn tại" };
                }
                else
                {
                    ret.isSuccess = false;
                    ret.id = id;
                    ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Thêm xe thất bại" };
                }
            }
            return ret;
        }

        [HttpPut("update")]
        public ActionMessage Update(Guid vehicleId, Guid driverId, Guid romoocId)
        {
            var ret = new ActionMessage();
            var username = GetUserId();
            if (vehicleId != Guid.Empty)
            {
                ret = VehicleServices.GetInstance().Update(username, vehicleId, driverId, romoocId);
            }
            else
            {
                ret.isSuccess = false;
                ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Parram Err!" };
            }
            return ret;
        }

        [HttpDelete("delete")]
        public ActionMessage Delete(Guid id)
        {
            var ret = new ActionMessage();
            var username = GetUserId();
            if (!(id == Guid.Empty))
            {
                var result = VehicleServices.GetInstance().Delete(username, id);
                if (result > 0)
                {
                    ret.isSuccess = true;
                    ret.id = result;
                    ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Xóa xe thành công" };
                }
                else
                {
                    ret.isSuccess = false;
                    ret.id = result;
                    ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Xóa xe thất bại" };
                }
            }
            return ret;
        }
    }
}