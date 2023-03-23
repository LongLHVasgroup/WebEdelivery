using System;
using System.Collections.Generic;
using System.Linq;
using WEB_KhaiBaoXeGiaoNhan.ADO;
using WEB_KhaiBaoXeGiaoNhan.Constants;
using WEB_KhaiBaoXeGiaoNhan.Datalayers;
using WEB_KhaiBaoXeGiaoNhan.Models;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Services
{
    public class OrdersServices : BaseService<OrdersServices>
    {
        /// <summary>
        /// lấy danh sách order theo công ty
        /// </summary>
        /// <param name="username"></param>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        public OrderResponseModel GetOrder(string username, string companyCode)
        {
            OrderResponseModel ret = null;

            string CnnString = CompanyService.GetInstance().GetConnStr(companyCode);



            {
                //tìm user
                using (var _context = new Web_BookingTransContext())
                {
                    var user = UserModelDAO.GetInstance().GetList()
                                                  .Where(u => u.Username == username)
                                                  .FirstOrDefault();
                    if (user != null)
                    {
                        //loại khách hàng
                        if (user.UserType == UserConstants.UserTypeC && user.IsService == false)
                        {
                            var customer = CustomerModelDAO.GetInstance().GetList()
                                                            .Where(c => c.CustomerId == user.Memberof)
                                                            .FirstOrDefault();
                            if (customer != null)
                            {
                                ret = new OrderResponseModel();

                                var SoNumbers = SoMasterModelDAO.GetInstance().GetList()
                                                            .Where(so => so.CustomerCode == customer.CustomerCode)
                                                            .Where(so => so.IsCompelete == false)
                                                            .Select(ln => ln.Sonumber)
                                                            .Distinct()
                                                            .ToList();
                                if (SoNumbers.Count > 0)
                                {
                                    var SoLines = SoLinesMasterDAO.GetInstance().GetList()
                                                                    .Where(soln => SoNumbers.Contains(soln.Sonumber))
                                                                    .ToList();

                                    var SoMasters = SoMasterModelDAO.GetInstance().GetList()
                                                            .Where(soln => SoNumbers.Contains(soln.Sonumber))
                                                            .ToList();
                                    foreach (var item in SoMasters)
                                    {
                                        SoResponseModel po = new SoResponseModel();
                                        po.Somasters = item;
                                        po.Solines = SoLines.Where(ln => ln.Sonumber == item.Sonumber)
                                                    .ToList();
                                        ret.soResponses.Add(po);
                                    }
                                }
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else if (user.UserType == UserConstants.UserTypeP && user.IsService == false)
                        {
                            //tìm nhà cung cấp
                            var provider = ProviderModelDAO.GetInstance().GetList()
                                                            .Where(p => p.ProviderId == user.Memberof)
                                                            .FirstOrDefault();
                            //tìm nhà cung cấp liên quan(nếu là nhà vận chuyển)

                            if (provider != null)
                            {
                                ret = new OrderResponseModel();

                                var PoNumbers = PoLinesModelDAO.GetInstance().GetList()
                                                               .Where(po => po.ProviderCode == provider.ProviderCode)
                                                               .Where(ln => ln.DeliveryDate >= DateTime.Today)
                                                               .Where(ln => ln.IsDeliveryCompleted != true)
                                                               .Select(ln => ln.Ponumber)
                                                               .Distinct()
                                                               .ToList();
                                
                                if (PoNumbers.Count > 0)
                                {
                                    List<PomasterModel> Pomaster;
                                    try
                                    {
                                        if (companyCode == "undefined")
                                        {
                                            Pomaster = PoMasterModelDAO.GetInstance().GetList()
                                                                    .Where(po => PoNumbers.Contains(po.Ponumber))
                                                                    .Where(po => !po.Note.Contains(PODuongThuyConstants.PODUONGTHUY))// Không hiển thị các PO đường thủy
                                                                    .ToList();
                                        }
                                        else
                                        {
                                            Pomaster = PoMasterModelDAO.GetInstance().GetList()
                                                                    .Where(po => PoNumbers.Contains(po.Ponumber))
                                                                    .Where(po => po.CompanyCode.Contains(companyCode))
                                                                    .Where(po => !po.Note.Contains(PODuongThuyConstants.PODUONGTHUY))// Không hiển thị các PO đường thủy
                                                                    .ToList();
                                        }

                                        // lấy những po đúng với CompanyCode truyền vào

                                    }
                                    catch (Exception ex)
                                    {
                                        // Lấy toàn bộ PO lên
                                        Pomaster = PoMasterModelDAO.GetInstance().GetList()
                                                                .Where(po => PoNumbers.Contains(po.Ponumber))
                                                                .ToList();
                                    }


                                    var Poline = PoLinesModelDAO.GetInstance().GetList()
                                                                .Where(ln => PoNumbers.Contains(ln.Ponumber))
                                                                .ToList();

                                    for (int i = 0; i < Pomaster.Count; i++)
                                    {
                                        PoResponseModel po = new PoResponseModel();
                                        if (PoProperties.CheckOrtherPrice(Pomaster[i].Note))
                                        {
                                            po.isGiaKhac = true;
                                        }
                                        else
                                        {
                                            po.isGiaKhac = false;
                                        }
                                        po.Pomasters = Pomaster[i];
                                        po.Polines = Poline.Where(ln => ln.Ponumber == Pomaster[i].Ponumber)
                                                    .ToList();
                                        //lấy theo trọng lượng đã nhập thực tế
                                        po.TrongLuongDaNhap = GetDataFromFunction.GetInstance().GetSLDaNhapTuPONumber(Pomaster[i].Ponumber, cnt: CnnString);
                                        //lấy theo trọng lượng đã đăng ký nhưng chưa giao
                                        var chuagiao = VehicleRegisterMobileModelDAO.GetInstance().GetList()
                                                    .Where(v => v.SoDonHang == Pomaster[i].Ponumber && (v.TrongLuongGiaoThucTe == 0 || v.TrongLuongGiaoThucTe == null))
                                                    .Sum(i => i.TrongLuongGiaoDuKien);

                                        po.Registered = chuagiao;
                                        //nếu đã nhập + đã khai báo lớn hơn tổng yêu cầu thì không cho đăng ký nữa
                                        // Cho phé vượt 10%

                                        //Lê Hoàng Long - Hiển thị PO chưa đóng
                                        //Nếu IsPmccompleted = 1 thì không cho hiển thị lên

                                        bool statusComplete = true;

                                        foreach (var item in po.Polines)
                                        {
                                            if (item.IsPmccompleted.HasValue)
                                            {
                                                if(item.IsPmccompleted.Value == true)
                                                {
                                                    statusComplete = false;
                                                }
                                            }
                                        }

                                         
                                        var soLuongPO = Pomaster[i].QtyTotal * (decimal)(1.1);
                                        if (po.TrongLuongDaNhap + po.Registered < soLuongPO && statusComplete)
                                            ret.poResponses.Add(po);
                                    }
                                }
                                else
                                {
                                    return null;
                                }
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else if (user.IsService == true)
                        {
                            ret = new OrderResponseModel();

                            //thông tin master
                            // var masterInfo = _context.OrderMapping.Where(or => or.ServiceId == user.Memberof).ToList();
                            var lstPOMappingModel = _context.OrderMapping.Where(or => or.ServiceId == user.Memberof).ToList();

                            if (lstPOMappingModel == null)
                            {
                                return null;
                            }
                            var lstPOMapping = lstPOMappingModel.Select(e => e.OrderNumber).ToList();

                            // List model poMaster theo plant
                            var lstPoMaster = PoMasterModelDAO.GetInstance().GetList()
                                                                .Where(po => lstPOMapping.Contains(po.Ponumber))
                                                                .Where(po => po.CompanyCode == companyCode)
                                                                .ToList();



                            // ds po theo đúng plant                                    
                            List<String> lstPONumber = lstPoMaster.Select(po => po.Ponumber).ToList();

                            //ds model điều phối theo đúng plant
                            lstPOMappingModel = lstPOMappingModel.Where(e => lstPONumber.Contains(e.OrderNumber)).ToList();

                            // Thông tin POLine
                            var Poline = PoLinesModelDAO.GetInstance().GetList()
                                                                .Where(e => lstPONumber.Contains(e.Ponumber))
                                                                .Where(e => e.DeliveryDate >= DateTime.Today)
                                                                .Where(ln => ln.IsDeliveryCompleted != null || ln.IsDeliveryCompleted == false)
                                                                .ToList();



                            foreach (var item in lstPOMappingModel)
                            {
                                PoResponseModel po = new PoResponseModel();
                                if (Poline.Where(e => e.Ponumber == item.OrderNumber).ToList().Count() > 0)
                                {
                                    //lấy thông tin po
                                    var pomaster = lstPoMaster.Where(e => e.Ponumber == item.OrderNumber).FirstOrDefault();
                                    //thay số lượng bằng số lượng được điều phối
                                    pomaster.QtyTotal = item.SoLuong;
                                    //check giá khác
                                    if (PoProperties.CheckOrtherPrice(pomaster.Note))
                                    {
                                        po.isGiaKhac = true;
                                    }
                                    else
                                    {
                                        po.isGiaKhac = false;
                                    }
                                    //thêm thông tin pomaster
                                    po.Pomasters = pomaster;
                                    //poline đi kèm
                                    po.Polines = Poline.Where(ln => ln.Ponumber == item.OrderNumber)
                                                .ToList();
                                    //đã giao thì lấy trọng lượng thật
                                    var dagiao = VehicleRegisterMobileModelDAO.GetInstance().GetList()
                                                .Where(v => v.SoDonHang == item.OrderNumber && (v.TrongLuongGiaoThucTe != 0 || v.TrongLuongGiaoThucTe != null))
                                                .Sum(i => i.TrongLuongGiaoThucTe - i.TapChat);
                                    po.TrongLuongDaNhap = dagiao == null ? 0 : dagiao;
                                    //chưa giao thì lấy trọng lượng đăng ký
                                    var chuagiao = VehicleRegisterMobileModelDAO.GetInstance().GetList()
                                                .Where(v => v.SoDonHang == item.OrderNumber && (v.TrongLuongGiaoThucTe == 0 || v.TrongLuongGiaoThucTe == null))
                                                .Sum(i => i.TrongLuongGiaoDuKien);
                                    //số lượng đã đăng ký chưa giao thì lấy chưa giao, đã giao thì lấy theo đã giao
                                    po.Registered = chuagiao;
                                    // Lấy thêm thông tin điều phối Bill, số cont, đơn hàng cont
                                    po.isCont = item.IsCont;
                                    po.soLuongCont = item.SoLuongCont;
                                    po.billNumber = item.BillNumber;
                                    po.shipNumber = item.ShipNumber;
                                    //nếu mà đã đăng ký lố thì không hiện nữa
                                    if (item.IsCont == true) // Xe chuyển cont thì không có trọng lượng ban đầu nên trả về luôn
                                    {
                                        ret.poResponses.Add(po);
                                    }
                                    else // Xe điều phối theo KG thì cần xét so lượng đã giao
                                    {


                                        //Lê Hoàng Long - Hiển thị PO chưa đóng
                                        //Nếu IsPmccompleted = 1 thì không cho hiển thị lên

                                        bool statusComplete = true;

                                        foreach (var statusItem in po.Polines)
                                        {
                                            if (statusItem.IsPmccompleted.HasValue)
                                            {
                                                if (statusItem.IsPmccompleted.Value == true)
                                                {
                                                    statusComplete = false;
                                                }
                                            }
                                        }


                                        if (po.TrongLuongDaNhap + po.Registered < item.SoLuong && statusComplete)
                                            ret.poResponses.Add(po);
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return ret;
        }
    }
}