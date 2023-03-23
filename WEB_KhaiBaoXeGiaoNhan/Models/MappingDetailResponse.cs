using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Models
{
    public class MappingDetailResponse
    {
        public MappingDetailResponse()
        {

        }
        public PomasterModel pomaster { get; set; }

        public decimal trongLuongDaNhap { get; set; }
        public Guid providerId { get; set; }
        public List<OrderMapping> mappings { get; set; }
        public List<PolineModel> polines { get; set; }
    }
}