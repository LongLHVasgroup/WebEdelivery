using System.Collections.Generic;
using WEB_KhaiBaoXeGiaoNhan.Datalayers;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Services
{
    public class CungDuongServices : BaseService<CungDuongServices>
    {
        public List<CungDuongModel> GetList()
        {
            List<CungDuongModel> ret = null;
            ret = CungDuongModelDAO.GetInstance().GetList();
            return ret;
        }
        public List<CungDuongModel> GetListByCompany(string companyCode)
        {
            List<CungDuongModel> ret = null;
            ret = CungDuongModelDAO.GetInstance().GetListByCompany(companyCode);
            return ret;
        }

        public List<CungDuongModel> GetListByUser(string username)
        {
            List<CungDuongModel> ret = null;
            ret = CungDuongModelDAO.GetInstance().GetListByUsername(username);
            return ret;
        }
    }
}