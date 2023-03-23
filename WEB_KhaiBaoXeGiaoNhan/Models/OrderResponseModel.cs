using System.Collections.Generic;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Models
{
    public class OrderResponseModel
    {
        public List<PoResponseModel> poResponses { get; set; }
        public List<SoResponseModel> soResponses { get; set; }

        public OrderResponseModel()
        {
            poResponses = new List<PoResponseModel>();
            soResponses = new List<SoResponseModel>();
        }
    }

    public class PoResponseModel
    {
        public PoResponseModel()
        {
            Polines = new List<PolineModel>();
        }

        public PomasterModel Pomasters { get; set; }
        public List<PolineModel> Polines { get; set; }
        public decimal? TrongLuongDaNhap { get; set; }
        public decimal? Registered { get; set; }
        public bool? isGiaKhac { get; set; }
        public bool? isCont { get; set; }
        public decimal? soLuongCont { get; set; }
        public string? billNumber { get; set; }
        public string? shipNumber { get; set; }
    }

    public class SoResponseModel
    {
        public SoResponseModel()
        {
            Solines = new List<SolineModel>();
        }

        public SomasterModel Somasters { get; set; }
        public List<SolineModel> Solines { get; set; }
    }
}