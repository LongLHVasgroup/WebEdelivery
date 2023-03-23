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
    public class MappingController : BaseController
    {
        [HttpGet("find")]
        public ListResponeMessage<OrderMapping> GetServices(string PoNumber)
        {
            var ret = new ListResponeMessage<OrderMapping>();
            var data = MappingServices.GetInstance().GetServices(PoNumber);
            if (data != null && data.Count >= 0)
            {
                ret.totalRecords = data.Count;
                ret.data = data;
                ret.isSuccess = true;
                ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Danh sách thông tin điều phối" };
            }
            else
            {
                ret.data = null;
                ret.isSuccess = false;
                ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Không có thông tin điêu phối" };
            }
            return ret;
        }

        [HttpGet("get-info")]
        public SingleResponeMessage<MappingDetailResponse> GetProvider2DieuPhoi(string PoNumber)
        {
            var ret = new SingleResponeMessage<MappingDetailResponse>();
            var username = GetUserId();
            var data = ProviderServices.GetInstance().SearchPO2DieuPhoi(PoNumber, username);
            ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Không tìm thấy thông tin đơn hàng" };
            ret.isSuccess = false;
            if (data != null)
            {
                ret.isSuccess = true;
                ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Chi tiết điều phối theo đơn hàng" };
            }


            ret.item = data;
            return ret;
        }

        // Cập nhật trạng thái điều phối xong
        [HttpPut("{id}")]
        public ActionMessage UpdateStatusDieuPhoi(Guid id)
        {
            var ret = new ActionMessage();
            var username = GetUserId();
            var status = MappingServices.GetInstance().ChangeIsDone(id, username);
            ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Không tìm thấy thông tin đơn hàng" };
            ret.isSuccess = false;
            if (status == 1)
            {
                ret.isSuccess = true;
                ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Cập nhật trạng thái điều phối thành công" };
            }

            return ret;
        }
    }
}