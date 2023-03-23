using AdminPortal.DataLayer;
using System.Collections.Generic;
using System.Linq;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Datalayers
{
    public class CustomerModelDAO : DataProvider<CustomerModelDAO>
    {
        public List<CustomerModel> GetList()
        {
            using (Web_BookingTransContext context = new Web_BookingTransContext())
            {
                return context.CustomerModel.ToList();
            }
        }
    }
}