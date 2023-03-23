using AdminPortal.DataLayer;
using System.Collections.Generic;
using System.Linq;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Datalayers
{
    public class PoLinesModelDAO : DataProvider<PoLinesModelDAO>
    {
        public List<PolineModel> GetList()
        {
            using (Web_BookingTransContext context = new Web_BookingTransContext())
            {
                return context.PolineModel.ToList();
            }
        }
    }
}