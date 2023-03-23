using Microsoft.AspNetCore.Mvc;
using Models.Common;
using System.Collections.Generic;
using WEB_KhaiBaoXeGiaoNhan.Models;
using WEB_KhaiBaoXeGiaoNhan.Responses;
using WEB_KhaiBaoXeGiaoNhan.Services;

namespace WEB_KhaiBaoXeGiaoNhan.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class RegisterForShipController : BaseController
    {
        /*
        [HttpPost]
        public ActionMessage RegisterPO([FromBody] RegisterForShipModel item)
        {
            var ret = new ActionMessage();
            var username = GetUserId();
            ret = VehicleRegisterForShipServices.GetInstance().RegisterPO(item, username);
            return ret;
        }

        [HttpDelete]
        public ActionMessage DeleteRegisForShip([FromQuery] string ponumber, string productCode)
        {
            var ret = new ActionMessage();
            var username = GetUserId();
            ret = VehicleRegisterForShipServices.GetInstance().DeleteRegister(ponumber, productCode, username);
            return ret;
        }

        [HttpDelete("{id}")]
        public ActionMessage DeleteRegisForShipSingle(int id)
        {
            var ret = new ActionMessage();
            var username = GetUserId();
            ret = VehicleRegisterForShipServices.GetInstance().DeleteRegisterSingle(id, username);
            return ret;
        }



        [HttpPut]
        public ActionMessage UpdateRegisForShip([FromBody] RegisterForShipModel item)
        {
            var ret = new ActionMessage();
            var username = GetUserId();
            ret = VehicleRegisterForShipServices.GetInstance().UpdateRegister(item, username);
            return ret;
        }

        [HttpGet]
        public ListResponeMessage<VehicleRegisForShipResponse> GetAllVehicleRegisForShip(string shipNumber, string orderNumber, string productCode)
        {
            var ret = new ListResponeMessage<VehicleRegisForShipResponse>();
            var username = GetUserId();
            var data = VehicleRegisterForShipServices.GetInstance().GetList(username, shipNumber, orderNumber, productCode);
            if (data.Count > 0)
            {
                ret.totalRecords = data.Count;
                ret.isSuccess = true;

                ret.data = data;
                ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Danh sách xe" };
            }
            else
            {
                ret.isSuccess = true;
                ret.data = new List<VehicleRegisForShipResponse>();
                ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Danh sách xe rỗng" };
            }
            return ret;
        }
        */
    }
}
