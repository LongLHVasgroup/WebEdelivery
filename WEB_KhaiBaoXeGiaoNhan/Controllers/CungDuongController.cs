using Microsoft.AspNetCore.Mvc;
using Models.Common;
using WEB_KhaiBaoXeGiaoNhan.Services;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CungDuongController : BaseController
    {
        [HttpGet]
        public ListResponeMessage<CungDuongModel> GetList(string CompanyCode)
        {
            var ret = new ListResponeMessage<CungDuongModel>();
            ret.isSuccess = true;
            if (CompanyCode == "undefied")
            {
                ret.data = CungDuongServices.GetInstance().GetList();
            }
            else
            {
                ret.data = CungDuongServices.GetInstance().GetListByCompany(CompanyCode);
            }
            ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Thông tin cung đường" };
            return ret;
        }

        [HttpGet("list")]
        public ListResponeMessage<CungDuongModel> GetByUserName()
        {
            var ret = new ListResponeMessage<CungDuongModel>();
            var username = GetUserId();
            ret.isSuccess = true;
            ret.data = CungDuongServices.GetInstance().GetListByUser(username);
            ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Danh sách cung đường" };
            return ret;
        }


        [HttpGet("find")]
        public ListResponeMessage<CungDuongModel> Find()
        {
            var ret = new ListResponeMessage<CungDuongModel>();
            ret.isSuccess = true;
            ret.data = CungDuongServices.GetInstance().GetList();
            ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Thông tin cung đường" };
            return ret;
        }
    }
}