using System;
using AdminPortal.DataLayer;
using System.Collections.Generic;
using System.Linq;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Datalayers
{
    public class CungDuongModelDAO : DataProvider<CungDuongModelDAO>
    {
        public List<CungDuongModel> GetList()
        {
            using (Web_BookingTransContext context = new Web_BookingTransContext())
            {
                return context.CungDuongModel.ToList();
            }
        }

        public List<CungDuongModel> GetListByCompany(string companyCode)
        {
            using (Web_BookingTransContext context = new Web_BookingTransContext())
            {
                return context.CungDuongModel
                                            .Where(cd => cd.Plant.Contains(companyCode))
                                            .ToList();
            }
        }


        public List<CungDuongModel> GetListByUsername(string username)
        {
            using (Web_BookingTransContext context = new Web_BookingTransContext())
            {
                var user = UserModelDAO.GetInstance().GetList()
                                                    .Where(u => u.Username == username).FirstOrDefault();
                if (user == null) return null;
                try
                {
                    return context.CungDuongModel.Where(cd => cd.Plant.Contains(user.CompanyCode)).ToList();
                }
                catch (Exception ex)
                {
                    WriteLogErr(ex.Message);
                    return null;
                }
            }
        }
    }
}