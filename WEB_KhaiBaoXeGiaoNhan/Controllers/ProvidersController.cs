using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_KhaiBaoXeGiaoNhan.Models;
using WEB_KhaiBaoXeGiaoNhan.Services;

namespace WEB_KhaiBaoXeGiaoNhan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvidersController : BaseController
    {
        /// <summary>
        /// Lấy danh sách Provider theo PO còn hạn
        /// </summary>
        /// <returns></returns>
        [HttpGet("find")]
        public ListResponeMessage<ProviderResponse> GetProvider()
        {
            var ret = new ListResponeMessage<ProviderResponse>();
            var data = ProviderServices.GetInstance().GetProvider();
            ret.isSuccess = true;
            ret.totalRecords = data.Count;
            ret.data = data;
            ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Danh sách nhà cung cấp" };
            return ret;
        }

        /**
        * Lấy DS các nhà cung cấp đang có PO còn hạn để điều phối cho các bên vận chuyển
        * Danh sách nhà cung cấp dựa trên companyCode của điều phối
        */
        [HttpGet("find2dieuphoi")]
        public ListResponeMessage<ProviderResponse> GetProvider2DieuPhoi()
        {
            var ret = new ListResponeMessage<ProviderResponse>();
            var username = GetUserId();
            var data = ProviderServices.GetInstance().GetProviderPOActiveByUser(username);
            ret.isSuccess = true;
            ret.totalRecords = data.Count;
            ret.data = data;
            ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Danh sách nhà cung cấp" };
            return ret;
        }
    }
}