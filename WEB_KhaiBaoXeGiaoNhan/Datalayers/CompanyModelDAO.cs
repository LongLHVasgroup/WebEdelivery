using AdminPortal.DataLayer;
using System.Collections.Generic;
using System.Linq;
using WEB_KhaiBaoXeGiaoNhan.WebModels;
using System.Threading.Tasks;

namespace WEB_KhaiBaoXeGiaoNhan.Datalayers
{
    public class CompanyModelDAO: DataProvider<CompanyModelDAO>
    {
        public List<CompanyModel> GetList()
        {
            using (Web_BookingTransContext context = new Web_BookingTransContext())
            {
                return context.CompanyModel.ToList();
            }
        }
    }
}
