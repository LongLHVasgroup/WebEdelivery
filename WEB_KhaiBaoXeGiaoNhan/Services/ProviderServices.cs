using System;
using System.Collections.Generic;
using System.Linq;
using WEB_KhaiBaoXeGiaoNhan.ADO;
using WEB_KhaiBaoXeGiaoNhan.Datalayers;
using WEB_KhaiBaoXeGiaoNhan.Models;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Services
{
    public class ProviderServices : BaseService<ProviderServices>
    {
        public List<ProviderResponse> GetProvider()
        {
            var ret = new List<ProviderResponse>();
            //lấy po còn hạn
            var PoNumbers = PoLinesModelDAO.GetInstance()
                                        .GetList()
                                        .Where(ln => ln.DeliveryDate >= DateTime.Now)
                                        .Select(ln => ln.Ponumber)
                                        .Distinct()
                                        .ToList();
            var ListProvider = PoLinesModelDAO.GetInstance()
                                        .GetList()
                                        .Where(ln => ln.DeliveryDate >= DateTime.Now)
                                        .Select(ln => ln.ProviderCode)
                                        .Distinct()
                                        .ToList();
            if (PoNumbers.Count > 0)
            {
                //lấy danh sách pomaster
                var Pomaster = PoMasterModelDAO.GetInstance().GetList()
                                            .Where(po => PoNumbers.Contains(po.Ponumber))
                                            .ToList();
                //lấy danh sách poline
                var Poline = PoLinesModelDAO.GetInstance().GetList()
                                            .Where(ln => PoNumbers.Contains(ln.Ponumber))
                                            .ToList();
                //lấy danh sách provider
                var Provider = ProviderModelDAO.GetInstance().GetList()
                                            .Where(p => ListProvider.Contains(p.ProviderCode))
                                            .ToList();
                foreach (var item in Provider)
                {
                    ProviderResponse po = new ProviderResponse();
                    po.PoInfo = new List<PoResponseModel>();
                    //nhà cung cấp
                    po.Provider = item;
                    //danh sách po theo từng nhà cung cấp
                    var lstPO = Pomaster.Where(p => p.ProviderCode == item.ProviderCode).ToList();
                    foreach (var pomaster in lstPO)
                    {
                        var temp = new PoResponseModel();
                        //poline
                        var poline = Poline.Where(ln => ln.Ponumber == pomaster.Ponumber).ToList();
                        temp.Pomasters = pomaster;
                        temp.Polines = poline;

                        //Tìm CnnStr để lấy số lượng đã nhập
                        //Lê Hoàng Long
                        string CnnString = CompanyService.GetInstance().GetConnStr(pomaster.CompanyCode);
                        temp.TrongLuongDaNhap = GetDataFromFunction.GetInstance().GetSLDaNhapTuPONumber(pomaster.Ponumber, cnt: CnnString);
                        //
                        po.PoInfo.Add(temp);
                    }
                    ret.Add(po);
                }
            }
            else
            {
                return null;
            }

            return ret;
        }

        public List<ProviderResponse> GetProviderPOActiveByUser(string username)
        {
            var ret = new List<ProviderResponse>();
            try
            {
                var user = UserModelDAO.GetInstance().GetList()
                                                    .Where(u => u.Username == username).FirstOrDefault();
                if (user != null)
                {
                    if (user.UserType == Constants.UserConstants.UserTypeCor)
                    {
                        //lấy po còn hạn
                        var PoNumbers = PoLinesModelDAO.GetInstance()
                                                    .GetList()
                                                    .Where(ln => ln.DeliveryDate >= DateTime.Now)
                                                    .Select(ln => ln.Ponumber)
                                                    .Distinct()
                                                    .ToList();

                        var PoNumberByCompany = PoMasterModelDAO.GetInstance()
                                                    .GetList()
                                                    .Where(poc => PoNumbers.Contains(poc.Ponumber))
                                                    .Where(poc => poc.CompanyCode == user.CompanyCode)
                                                    .Select(poc => poc.Ponumber)
                                                    .Distinct()
                                                    .ToList();

                        var ListProvider = PoLinesModelDAO.GetInstance()
                                                    .GetList()
                                                    .Where(ln => ln.DeliveryDate >= DateTime.Now)
                                                    .Select(ln => ln.ProviderCode)
                                                    .Distinct()
                                                    .ToList();
                        if (PoNumberByCompany.Count > 0)
                        {
                            //lấy danh sách pomaster
                            var Pomaster = PoMasterModelDAO.GetInstance().GetList()
                                                        .Where(po => PoNumberByCompany.Contains(po.Ponumber))
                                                        .Where(po => po.CompanyCode.Contains(user.CompanyCode))
                                                        .ToList();
                            //lấy danh sách poline
                            var Poline = PoLinesModelDAO.GetInstance().GetList()
                                                        .Where(ln => PoNumberByCompany.Contains(ln.Ponumber))
                                                        .ToList();
                            //lấy danh sách provider
                            var Provider = ProviderModelDAO.GetInstance().GetList()
                                                        .Where(p => ListProvider.Contains(p.ProviderCode))
                                                        .ToList();
                            foreach (var item in Provider)
                            {
                                ProviderResponse po = new ProviderResponse();
                                po.PoInfo = new List<PoResponseModel>();
                                //nhà cung cấp
                                po.Provider = item;
                                //danh sách po theo từng nhà cung cấp
                                var lstPO = Pomaster.Where(p => p.ProviderCode == item.ProviderCode).ToList();
                                foreach (var pomaster in lstPO)
                                {
                                    var temp = new PoResponseModel();
                                    //poline
                                    var poline = Poline.Where(ln => ln.Ponumber == pomaster.Ponumber).ToList();
                                    temp.Pomasters = pomaster;
                                    temp.Polines = poline;

                                    //Tìm CnnStr để lấy số lượng đã nhập
                                    //Lê Hoàng Long
                                    string CnnString = CompanyService.GetInstance().GetConnStr(pomaster.CompanyCode);

                                    temp.TrongLuongDaNhap = GetDataFromFunction.GetInstance().GetSLDaNhapTuPONumber(pomaster.Ponumber, cnt: CnnString);
                                    //
                                    po.PoInfo.Add(temp);
                                }
                                ret.Add(po);
                            }
                        }
                        else
                        {
                            return ret;
                        }

                        return ret;
                    }
                    else
                    {
                        return null;
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            return ret;

        }
        public MappingDetailResponse SearchPO2DieuPhoi(string orderNumber, string username)
        {
            var ret = new MappingDetailResponse();
            try
            {
                var user = UserModelDAO.GetInstance().GetList()
                                                    .Where(u => u.Username == username).FirstOrDefault();
                if (user == null)
                {
                    return null;
                }
                if (user.UserType == Constants.UserConstants.UserTypeCor) // Là điều phối viên
                {
                    //lấy po còn hạn
                    /*
                    Nội dung trả về
                    - thông tin master của po
                    - thông tin poline
                    - thông tin khác như trọng lượng
                    - thông tin điều phối

                    */
                    var poMasterInfo = PoMasterModelDAO.GetInstance()
                                                .GetList()
                                                .Where(po => po.Ponumber == orderNumber)
                                                .Where(po => po.CompanyCode == user.CompanyCode)
                                                .FirstOrDefault();



                    if (poMasterInfo == null)
                    {
                        return null;
                    }


                    var providerId = ProviderModelDAO.GetInstance().GetList()
                                                        .Where(p => p.ProviderCode == poMasterInfo.ProviderCode)
                                                        .Select(p => p.ProviderId)
                                                        .FirstOrDefault();

                    List<PolineModel> lstPoLine = PoLinesModelDAO.GetInstance().GetList().Where(line => line.Ponumber == poMasterInfo.Ponumber).ToList();


                    //Tìm CnnStr để lấy số lượng đã nhập
                    //Lê Hoàng Long
                    string CnnString = CompanyService.GetInstance().GetConnStr(poMasterInfo.CompanyCode);


                    // Trọng lượng đã giao
                    var TrongLuongDaNhap = GetDataFromFunction.GetInstance().GetSLDaNhapTuPONumber(poMasterInfo.Ponumber, cnt: CnnString);
                    // thông tin điều phối

                    var lstMapping = OrderMappingModelDAO.GetInstance().GetList().Where(map => map.OrderNumber == poMasterInfo.Ponumber).ToList();

                    ret.pomaster = poMasterInfo;
                    ret.polines = lstPoLine;
                    ret.mappings = lstMapping;
                    ret.trongLuongDaNhap = TrongLuongDaNhap;
                    ret.providerId = providerId;
                    return ret;
                }
                else
                {
                    return null;
                }


            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}