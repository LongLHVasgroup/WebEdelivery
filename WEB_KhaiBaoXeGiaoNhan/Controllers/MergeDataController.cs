using Microsoft.AspNetCore.Mvc;
using Models.Common;
using WEB_KhaiBaoXeGiaoNhan.Services;

namespace WEB_KhaiBaoXeGiaoNhan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MergeDataController : BaseController
    {
        /**
        Cập nhật danh sách các provider từ các plant về web
        */

        [HttpGet("provider")]
        public SingleResponeMessage<MergeDataResult> MergeProvider2Web(string plant)
        {
            var ret = MergeDataServices.GetInstance().MergeProvider(plant);
            return ret;
        }
    }
}