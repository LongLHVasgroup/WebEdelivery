using Microsoft.AspNetCore.Mvc;
using Models.Common;
using WEB_KhaiBaoXeGiaoNhan.Services;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CompanyController : BaseController
    {
        [HttpGet]
        public ListResponeMessage<CompanyModel> GetList()
        {
            var ret = new ListResponeMessage<CompanyModel>();
            var data = CompanyService.GetInstance().GetList();
            ret.isSuccess = false;
            ret.data = data;
            ret.totalRecords = 0;
            ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Không lấy được danh sách" };
            if (data != null)
            {
                ret.isSuccess = true;
                ret.totalRecords = data.Count;
                ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Danh sách Cty con" };
            }
            return ret;
        }
    }
}