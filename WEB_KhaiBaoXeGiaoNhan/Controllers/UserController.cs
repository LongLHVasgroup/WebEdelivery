using Microsoft.AspNetCore.Mvc;
using Models.Common;
using WEB_KhaiBaoXeGiaoNhan.Models;
using WEB_KhaiBaoXeGiaoNhan.Services;

namespace WEB_KhaiBaoXeGiaoNhan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        [HttpGet("{username}")]
        public SingleResponeMessage<UserResponseModel> GetInfo(string username)
        {
            SingleResponeMessage<UserResponseModel> ret = new SingleResponeMessage<UserResponseModel>();
            var item = UserService.GetInstance().GetUserInfo(username);
            if (item != null)
            {
                ret.isSuccess = true;
                ret.item = item;
                ret.err.msgCode = "2xx";
                ret.err.msgString = "Lấy thông tin thành công";
            }
            else
            {
                ret.isSuccess = false;
                ret.item = null;
                ret.err.msgCode = "4xx";
                ret.err.msgString = "Thiếu dữ liệu";
            }
            return ret;
        }

        [HttpPut("password/change")]
        public ActionMessage UpdatePassword([FromBody] UpdateModel item)
        {
            var ret = new ActionMessage();
            if (item.Username.Equals(GetUserId()))
            {
                ret = UserService.GetInstance().PasswordChange(item);
            }
            else
            {
                ret.isSuccess = false;
                ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Không có quyền thực thi" };
            }
            return ret;
        }

        [HttpPut("info/change")]
        public ActionMessage UpdateInfo([FromBody] UpdateInfoModel item)
        {
            var ret = new ActionMessage();
            if (item != null)
            {
                ret = UserService.GetInstance().UpdateInfo(item, GetUserId());
            }
            else
            {
                ret.isSuccess = false;
                ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Unknow Error" };
            }
            return ret;
        }
    }
}