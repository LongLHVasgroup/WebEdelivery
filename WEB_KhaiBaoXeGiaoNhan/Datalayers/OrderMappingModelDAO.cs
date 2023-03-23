using AdminPortal.DataLayer;
using System.Collections.Generic;
using System.Linq;
using WEB_KhaiBaoXeGiaoNhan.WebModels;
using System.Threading.Tasks;

namespace WEB_KhaiBaoXeGiaoNhan.Datalayers
{
    public class OrderMappingModelDAO: DataProvider<OrderMappingModelDAO>
    {
        public List<OrderMapping> GetList()
        {
            using (Web_BookingTransContext context = new Web_BookingTransContext())
            {
                return context.OrderMapping.ToList();
            }
        }
    }
}
