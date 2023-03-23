using System.Collections.Generic;
using WEB_KhaiBaoXeGiaoNhan.WebModels;
using WEB_KhaiBaoXeGiaoNhan.Datalayers;
using Models.Common;

namespace WEB_KhaiBaoXeGiaoNhan.Services
{
    public class CompanyService: BaseService<CompanyService>
    {
        public List<CompanyModel> GetList()
        {
            List<CompanyModel> ret = null;
            ret = CompanyModelDAO.GetInstance().GetList();
            return ret;
        }

        public string GetConnStr(string CompanyCode)
        {
            string CnnString = "";
            switch (CompanyCode)
            {
                case "3000":
                    CnnString = Config.getInstance().connPMC3000;
                    break;
                case "4000":
                    CnnString = Config.getInstance().connPMC;
                    break;
                case "6000":
                    CnnString = Config.getInstance().connPMC6000;
                    break;
                case "7000":
                    break;
                default:
                    CnnString = Config.getInstance().connPMC;
                    break;
            }
            return CnnString;
        }
    }
}
