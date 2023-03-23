using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_KhaiBaoXeGiaoNhan.Models;
using WEB_KhaiBaoXeGiaoNhan.Services;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleServicesController : BaseController
    {
        /// <summary>
        /// Tìm nhà vận chuyển
        /// </summary>
        /// <returns></returns>
        [HttpGet("find")]
        public ListResponeMessage<ProviderModel> GetList()
        {
            var ret = new ListResponeMessage<ProviderModel>();
            var data = VehicleCoordinatorServices.GetInstance().GetVehicle();
            if (data != null)
            {
                ret.data = data;
                ret.isSuccess = true;
                ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Danh sách nhà vận chuyển" };
            }
            else
            {
                ret.data = null;
                ret.isSuccess = false;
                ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Không lấy được danh sách" };
            }
            return ret;
        }
        /**
         * Điều phối PO cho DVVC
         */
        [HttpPost("map")]
        public ActionMessage MappingPo([FromBody] MappingModel value)
        {
            var ret = new ActionMessage();
            ret = VehicleCoordinatorServices.GetInstance().MappingPo(value);
            return ret;
        }
    }
}