using Microsoft.AspNetCore.Mvc;
using Models.Common;
using System;
using System.Linq;
using WEB_KhaiBaoXeGiaoNhan.Services;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : BaseController
    {
        [HttpGet]
        public ListResponeMessage<DriverRegister> GetList()
        {
            var ret = new ListResponeMessage<DriverRegister>();
            var username = GetUserId();
            var data = DriverRegisterServices.GetInstance().GetList(username).ToList();
            if (data != null)
            {
                ret.data = data;
                ret.isSuccess = true;
                ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Lấy thông tin thành công" };
            }
            else
            {
                ret.data = null;
                ret.isSuccess = false;
                ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Lấy thông tin thất bại" };
            }
            return ret;
        }

        [HttpGet("find")]
        public ListResponeMessage<DriverRegister> Find(string criteria = "")
        {
            var ret = new ListResponeMessage<DriverRegister>();
            var username = GetUserId();
            var data = DriverRegisterServices.GetInstance().GetList(username, criteria).ToList();
            if (data.Count > 0)
            {
                ret.data = data;
                ret.isSuccess = true;
                ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Lấy thông tin thành công" };
            }
            else
            {
                ret.data = null;
                ret.isSuccess = false;
                ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Lấy thông tin thất bại" };
            }
            return ret;
        }

        [HttpPost("dangki")]
        public ActionMessage CreateNew([FromBody] DriverRegister item)
        {
            var ret = new ActionMessage();
            var id = DriverRegisterServices.GetInstance().CreateNew(item, GetUserId());
            if (id > 0)
            {
                ret.id = id;
                ret.isSuccess = true;
                ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Thêm thông tin tài xế thành công" };
            }
            else
            {
                ret.id = id;
                ret.isSuccess = false;
                ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Thêm thông tin tài xế không thành công" };
            }
            return ret;
        }

        [HttpPut("capnhatthongtin")]
        public ActionMessage Update([FromBody] DriverRegister item)
        {
            var ret = new ActionMessage();
            var id = DriverRegisterServices.GetInstance().Update(item);
            if (id > 0)
            {
                ret.id = id;
                ret.isSuccess = true;
                ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Cập nhật thông tin tài xế thành công" };
            }
            else
            {
                ret.id = id;
                ret.isSuccess = false;
                ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Cập nhật thông tin tài xế không thành công" };
            }
            return ret;
        }

        [HttpDelete("delete")]
        public ActionMessage Delete(Guid ids)
        {
            var ret = new ActionMessage();
            var id = DriverRegisterServices.GetInstance().Delete(ids);
            if (id > 0)
            {
                ret.id = id;
                ret.isSuccess = true;
                ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Xóa thông tin tài xế thành công" };
            }
            else
            {
                ret.id = id;
                ret.isSuccess = false;
                ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Xóa thông tin tài xế không thành công" };
            }
            return ret;
        }
    }
}