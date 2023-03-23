using Microsoft.AspNetCore.Mvc;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using WEB_KhaiBaoXeGiaoNhan.Models;
using WEB_KhaiBaoXeGiaoNhan.Services;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleRegisterController : BaseController
    {
        [HttpPost("po")]
        public ActionMessage RegisterPO([FromBody] RegisterInfoModel item)
        {
            var ret = new ActionMessage();
            var username = GetUserId();
            ret = VehicleRegisterMobileServices.GetInstance().RegisterPO(item, username);
            return ret;
        }

        [HttpGet("find")]
        public ListResponeMessage<VehicleMobileResponse> RegisterPO(DateTime from, DateTime to, string po, string plant, bool isScales)
        {
            var ret = new ListResponeMessage<VehicleMobileResponse>();
            var username = GetUserId();
            var data = VehicleRegisterMobileServices.GetInstance().GetList(username, from, to, plant);
            if (data.Count > 0)
            {
                ret.totalRecords = data.Count;
                ret.isSuccess = true;
                // Nếu tìm kiếm tất cả
                if (po == null || po == "all")
                {
                    ret.data = data
                                //.Where(d => (d.Item.ThoiGianToiDuKien >= from && d.Item.ThoiGianToiDuKien <= to.AddDays(1)) || (from == DateTime.MinValue || to == DateTime.MinValue))
                                .Where(d => !(d.Item.AllowEdit == isScales))
                                .OrderByDescending(d => d.Item.RegisterTime)
                                .ToList();
                }
                else // Tìm theo PO
                {
                    ret.data = data
                                // .Where(d => (d.Item.ThoiGianToiDuKien >= from && d.Item.ThoiGianToiDuKien <= to.AddDays(1)) || (from == DateTime.MinValue || to == DateTime.MinValue))
                                .Where(d => d.Item.SoDonHang == po)
                                .Where(d => !(d.Item.AllowEdit == isScales))
                                .OrderByDescending(d => d.Item.RegisterTime)
                                .ToList();
                }

                ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Danh sách xe" };
            }
            else
            {
                ret.isSuccess = true;
                ret.data = new List<VehicleMobileResponse>();
                ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Danh sách xe rỗng" };
            }
            return ret;
        }

        [HttpDelete("delete/{id}")]
        public ActionMessage DeletePo(Guid id)
        {
            var ret = new ActionMessage();
            ret = VehicleRegisterMobileServices.GetInstance().Delete(id);
            return ret;
        }
        [HttpPost("delete/records")]
        public ActionMessage MultipleDelete([FromBody] Guid[] items)
        {
            var ret = new ActionMessage();
            ret = VehicleRegisterMobileServices.GetInstance().DeleteItems(items, GetUserId());
            return ret;
        }

        [HttpPut("update")]
        public ActionMessage UpdateDetail([FromBody] VehicleUpdateModel item, Guid id)
        {
            var ret = new ActionMessage();
            ret = VehicleRegisterMobileServices.GetInstance().Update(item, id, GetUserId());
            return ret;
        }

        [HttpPut("update/active")]
        public ActionMessage ActiveVehicleRegis(Guid id)
        {
            var ret = new ActionMessage();
            ret = VehicleRegisterMobileServices.GetInstance().ActiveVehicleRegis(id, GetUserId());
            return ret;
        }
    }
}