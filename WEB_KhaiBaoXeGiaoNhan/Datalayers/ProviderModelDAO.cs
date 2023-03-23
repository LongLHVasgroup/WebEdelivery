using AdminPortal.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Datalayers
{
    public class ProviderModelDAO : DataProvider<ProviderModelDAO>
    {
        public List<ProviderModel> GetList()
        {
            using (Web_BookingTransContext context = new Web_BookingTransContext())
            {
                return context.ProviderModel.ToList();
            }
        }
        public ProviderModel getProviderById(Guid id)
        {
            using (Web_BookingTransContext context = new Web_BookingTransContext())
            {
                return context.ProviderModel.Where(e => e.ProviderId == id).FirstOrDefault();
            }
        }
    }
}