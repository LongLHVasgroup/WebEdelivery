using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Models
{
    public class ProviderResponse
    {
        public ProviderResponse()
        {
            PoInfo = new List<PoResponseModel>();
        }

        public ProviderModel Provider { get; set; }
        public List<PoResponseModel> PoInfo { get; set; }
    }
}