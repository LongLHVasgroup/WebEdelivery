using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Models.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using WEB_KhaiBaoXeGiaoNhan.Constants;
using WEB_KhaiBaoXeGiaoNhan.Datalayers;
using WEB_KhaiBaoXeGiaoNhan.Models;
using WEB_KhaiBaoXeGiaoNhan.WebModels;
using WEB_KhaiBaoXeGiaoNhan.WebModelsPMC;
using System.Data.OleDb;
using WEB_KhaiBaoXeGiaoNhan.ADO;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System.Globalization;
using WEB_KhaiBaoXeGiaoNhan.VAS3000;

namespace WEB_KhaiBaoXeGiaoNhan.Services
{
    public class VehicleRegisterMobileServices : BaseService<VehicleRegisterMobileServices>
    {
        public static string specifier = "N";
        public static CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

        /// <summary>
        /// thêm xe khai báo giao/nhận
        /// </summary>
        /// <param name="model"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public ActionMessage RegisterPO(RegisterInfoModel model, string username)
        {
            var ret = new ActionMessage();
            if (model == null)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "4xx";
                ret.err.msgString = "Dữ liệu vào sai";
                return ret;
            }

            if (model.ListVehicle.Count() <= 0)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "4xx";
                ret.err.msgString = "Thiếu danh sách xe";
                return ret;
            }

            // check có trùng số xe với bên khai báo tàu hay không

            var lstVehiclenNumber = model.ListVehicle.Select(e => e.VehicleNumber).ToList();
            var vehicleTrungBienSo = 0;


            using (var _context = new Web_BookingTransContext())
            {
                vehicleTrungBienSo = _context.RegisterForShip.Where(e => lstVehiclenNumber.Contains(e.VehicleNumber))
                                            .Where(e => e.Status == true)
                                            .Count();
                if (vehicleTrungBienSo != 0)
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "4xx";
                    ret.err.msgString = $"Có xe đã được khai báo vận chuyển từ tàu, vui lòng kiểm tra lại";
                    return ret;
                }
            }

            var user = UserModelDAO.GetInstance().GetList()
                                                       .Where(u => u.Username == username)
                                                       .FirstOrDefault();


            var provider = ProviderModelDAO.GetInstance().GetList()
                                                    .Where(p => p.ProviderId == user.Memberof)
                                                    .FirstOrDefault();


            //số lượng nhập cho phép của po
            var pomaster = PoMasterModelDAO.GetInstance().GetList()
                                                .Where(p => p.Ponumber == model.OrderNumber)
                                                .FirstOrDefault();

            var plant = pomaster.CompanyCode;

            var companyCode = pomaster.CompanyCode; // lấy companyCode từ poMaster gắn vào đăng ký
            var poqantity = pomaster.QtyTotal;
            var accept = (poqantity / 100) * 10;
            var total = poqantity + accept;
            //số lượng đã nhập của po

            //Lê Hoàng Long
            //Lấy conectstring
            string CnnString = CompanyService.GetInstance().GetConnStr(companyCode);
            var trongluongdanhap = GetDataFromFunction.GetInstance().GetSLDaNhapTuPONumber(model.OrderNumber, cnt: CnnString);
            //số lượng po đã đăng ký
            var totalRegisted = VehicleRegisterMobileModelDAO.GetInstance().GetList()
                                                .Where(v => v.SoDonHang == model.OrderNumber)
                                                .Where(v => v.TrongLuongGiaoThucTe == null || v.TrongLuongGiaoThucTe == 0)
                                                .Sum(v => v.TrongLuongGiaoDuKien);


            //tính tổng trọng lượng xe đăng ký
            decimal s = 0;
            for (int i = 0; i < model.ListVehicle.Count; i++)
            {
                // mặc định là khai báo 1 dòng
                if (model.ListVehicle[i].SoLuot == 0)
                {
                    model.ListVehicle[i].SoLuot = 1;
                }
                // tối đa cho phép là 15 dòng
                if (model.ListVehicle[i].SoLuot > 15)
                {
                    model.ListVehicle[i].SoLuot = 15;
                }

                var removeCount = model.ListVehicle.Count - (i + 1);
                s += model.ListVehicle[i].TrongLuongDuKien * model.ListVehicle[i].SoLuot;
                var tempWeight = total - (trongluongdanhap + totalRegisted + s);
                if (tempWeight < 0)
                {
                    model.ListVehicle.RemoveRange(i + 1, removeCount);
                    break;
                }
            }

            // Thêm vào từng xe
            foreach (var item in model.ListVehicle)
            {

                var driver = DriverRegisterDAO.GetInstance().GetList()
                                                       .Where(d => d.DriverId == item.DriverID)
                                                       .FirstOrDefault();
                var vehicleInfo = new WebModels.VehicleRegisterMobileModel
                {
                    UserRegisterId = user.Userid,
                    RegisterTime = DateTime.Now,
                    ThoiGianToiDuKien = model.NgayGiaoNhan.AddHours(23).AddMinutes(59).AddSeconds(59),
                    Dvvc = provider.ProviderName,
                    Dvvccode = provider.ProviderCode,
                    SoDonHang = model.OrderNumber,
                    GiaoNhan = GiaoNhan.giao,
                    VehicleNumber = item.VehicleNumber,
                    DriverIdCard = driver.DriverCardNo,
                    DriverName = driver.DriverName,
                    TrongLuongGiaoDuKien = item.TrongLuongDuKien,
                    BonusHour = 8,
                    Romooc = item.Romooc,
                    CompanyCode = companyCode,
                    Note = item.Note,
                    Assets = item.Assets,
                    AllowEdit = true,
                };

                //Cung đường mặc định
                switch (plant)
                {
                    case "3000":
                        vehicleInfo.CungDuongName = Constants.CungDuongDefault.plant3000;
                        break;
                    case "4000":
                        vehicleInfo.CungDuongName = Constants.CungDuongDefault.plant4000;
                        break;
                    case "6000":
                        vehicleInfo.CungDuongName = Constants.CungDuongDefault.plant6000;
                        break;
                    default:
                        vehicleInfo.CungDuongName = "";
                        break;
                }


                // Check nếu là dvvc và điều phối po chung tàu
                if ((bool)user.IsService)
                {
                    OrderMapping poDieuPhoi = new OrderMapping();
                    using (var _context = new Web_BookingTransContext())
                    {
                        poDieuPhoi = _context.OrderMapping.Where(p => p.OrderNumber == model.OrderNumber).FirstOrDefault();
                    }
                    DateTime dateStart = (DateTime)poDieuPhoi.CreatedTime;
                    dateStart = dateStart.AddDays(-1);
                    DateTime dateEnd = (DateTime)poDieuPhoi.CreatedTime;
                    dateEnd = dateEnd.AddDays(1);


                    OrderMapping mapingInfo = new OrderMapping();
                    using (var _context = new Web_BookingTransContext())
                    {
                        mapingInfo = _context.OrderMapping.Where(e => e.ServiceId == user.Memberof)
                                                    .Where(e => e.OrderNumber == model.OrderNumber)
                                                    .FirstOrDefault();
                    }

                    if (mapingInfo != null)
                    {
                        var cungduong = CungDuongModelDAO.GetInstance().GetList()
                                                        .Where(c => c.CungDuongCode == mapingInfo.CungDuongCode)
                                                        .FirstOrDefault();// TODO: lấy cung đường theo plant
                        if (cungduong != null)
                        {
                            vehicleInfo.CungDuongCode = cungduong.CungDuongCode;
                            vehicleInfo.CungDuongName = cungduong.CungDuongName;
                        }
                    }

                    ret = NCCKhaiBaoXe(vehicleInfo, model.OrderNumber, companyCode, item);
                }
                else
                {
                    // NCC khai báo xe
                    ret = NCCKhaiBaoXe(vehicleInfo, model.OrderNumber, companyCode, item);
                }

            }

            return ret;
        }


        private ActionMessage NCCKhaiBaoXe(WebModels.VehicleRegisterMobileModel vehicleInfo, string orderNumber, string companyCode, RegisterDetailModel item)
        {
            var ret = new ActionMessage();
            //nếu duy nhất 1 xe thì tự động active, nếu chở cho nhiều po tự động deactive tất cả chờ active bằng tay
            /*
             * Trường hợp cần active bằng tay [Điều kiện mặc định là có cùng ngày giao]
             * 1 xe đăng ký cho nhiều PO khác nhau trên cùng plant
             */

            List<WebModels.VehicleRegisterMobileModel> lst = new List<WebModels.VehicleRegisterMobileModel>();

            using (var _context = new Web_BookingTransContext())
            {
                lst = _context.VehicleRegisterMobileModel//.Where(e => e.Dvvccode == vehicleInfo.Dvvccode)
                                                     .Where(e => e.SoDonHang != vehicleInfo.SoDonHang)
                                                     .Where(e => e.VehicleNumber == vehicleInfo.VehicleNumber)
                                                     .Where(e => e.ThoiGianToiDuKien == vehicleInfo.ThoiGianToiDuKien) // e => e.ThoiGianToiDuKien >= DateTime.Today
                                                     .Where(e => e.AllowEdit == true)
                                                     .Where(e => e.CompanyCode == vehicleInfo.CompanyCode) // đăng ký giao khác plant
                                                     .ToList();
            }

            if (lst.Count > 0 && lst != null)
            {
                vehicleInfo.IsActive = false;

                using (var _context = new Web_BookingTransContext())
                {
                    using (var trans = _context.Database.BeginTransaction())
                    {
                        for (int i = 0; i < lst.Count; i++)
                        {
                            lst[i].IsActive = false;
                            _context.Update(lst[i]);
                            // Cập nhật qua PMC ====
                            UpdateVehicleRegisterToPMC(lst[i], companyCode);
                            //=====
                            _context.SaveChanges();
                        }
                        trans.Commit();
                    }
                }
            }
            else
            {
                vehicleInfo.IsActive = true;
            }

            // thêm n  dòng vào dựa trên số  lượt
            for (int i = 0; i < item.SoLuot; i++)
            {
                var idmobile = Guid.NewGuid();
                vehicleInfo.VehicleRegisterMobileId = idmobile;
                if (VehicleRegisterMobileModelDAO.GetInstance().InsertOne(vehicleInfo) > 0)
                {
                    var result = 0;
                    //insert vào bảng pmc
                    InsertVehicleResigterToPMC(vehicleInfo, companyCode);

                    var Po = PoMasterModelDAO.GetInstance().GetList().Where(p => p.Ponumber == orderNumber).FirstOrDefault();

                    var PoLn = PoLinesModelDAO.GetInstance().GetList().Where(ln => ln.Ponumber == orderNumber).ToList();
                    foreach (var line in item.ListProduct)
                    {
                        var ln = new WebModels.VehicleRegisterPodetailModel();
                        ln.VehicleRegisterPodetailId = Guid.NewGuid();
                        ln.VehicleRegisterMobileId = idmobile;
                        ln.Ponumber = orderNumber;
                        ln.Poline = PoLn.Where(l => l.ProductCode == line)
                                        .Select(l => l.Poline)
                                        .FirstOrDefault();
                        ln.ProductCode = line;
                        ln.ProductName = PoLn.Where(l => l.ProductCode == line)
                                        .Select(l => l.ProductName)
                                        .FirstOrDefault();
                        result += VehicleRegisterPODetailModelDAO.GetInstance().InsertOne(ln);
                        //insert vào bảng pmc
                        InsertVehicleRegisterDetailToPMC(ln, companyCode);
                    }

                    ret.isSuccess = true;
                    ret.err.msgCode = "2xx";
                    ret.err.msgString = "Thêm thành công";
                }
                else
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "4xx";
                    ret.err.msgString = "Thêm thất bại";
                }
            }
            return ret;
        }


        /// <summary>
        /// danh sách xe đã khai báo giao nhận
        /// </summary>
        /// <param name="username"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="plant"></param>
        /// <returns></returns>
        public List<VehicleMobileResponse> GetList(string username, DateTime from, DateTime to, string plant)
        {
            using (var _context = new Web_BookingTransContext())
            {
                var ret = new List<VehicleMobileResponse>();
                //tìm user
                var user = UserModelDAO.GetInstance().GetList()
                                        .Where(u => u.Username == username).FirstOrDefault();
                var listVehicle = VehicleModelDAO.GetInstance().GetList();
                if (user != null)
                {
                    //tìm provider của user
                    var provider = ProviderModelDAO.GetInstance().GetList()
                                                    .Where(p => p.ProviderId == user.Memberof).FirstOrDefault();
                    //tìm danh sách po theo mã provider
                    var ponumber = PoLinesModelDAO.GetInstance().GetList()
                                                    .Where(po => po.ProviderCode == provider.ProviderCode)
                                                    //.Where(po => po.DeliveryDate >= DateTime.Today.AddDays(-1))
                                                    .Select(e => e.Ponumber)
                                                    .Distinct()
                                                    .ToList();
                    if (user.IsService == true)
                    {
                        var poMapping = _context.OrderMapping.Where(e => e.ServiceId == user.Memberof).Select(e => e.OrderNumber).ToList();
                        if (poMapping.Count > 0)
                        {
                            ponumber.AddRange(poMapping);
                        }
                    }

                    //Lê Hoàng Long
                    //Loại bỏ PO trùng
                    ponumber = ponumber.Distinct().ToList();

                    #region master data

                    var vehicleRegisterPODetailModelDAO = VehicleRegisterPODetailModelDAO.GetInstance().GetList();
                    var polines = PoLinesModelDAO.GetInstance()
                                                            .GetList();
                    var driverInfo = DriverRegisterDAO.GetInstance()
                                                                .GetList();

                    #endregion master data


                    var vehicleRegisterMobileModelData = VehicleRegisterMobileModelDAO.GetInstance().GetList();
                    if (plant != "all")
                    {
                        vehicleRegisterMobileModelData = vehicleRegisterMobileModelData.Where((d) => d.CompanyCode == plant).ToList();
                    }

                    for (int i = 0; i < ponumber.Count; i++)
                    {
                        var collection = new List<WebModels.VehicleRegisterMobileModel>();
                        if (user.IsService == true)
                        {
                            collection = vehicleRegisterMobileModelData
                                                                    .Where(v => v.SoDonHang == ponumber[i] && v.Dvvccode == provider.ProviderCode)
                                                                    // .Where(v => v.ThoiGianToiDuKien >= DateTime.Today)
                                                                    .Where(d => (d.ThoiGianToiDuKien >= from && d.ThoiGianToiDuKien <= to.AddDays(1)) || (from == DateTime.MinValue || to == DateTime.MinValue))
                                                                    .ToList();
                        }
                        else
                        {
                            collection = vehicleRegisterMobileModelData
                                                                    .Where(v => v.SoDonHang == ponumber[i])
                                                                    // .Where(v => v.ThoiGianToiDuKien >= DateTime.Today)
                                                                    .Where(d => (d.ThoiGianToiDuKien >= from && d.ThoiGianToiDuKien <= to.AddDays(1)) || (from == DateTime.MinValue || to == DateTime.MinValue))
                                                                    .ToList();
                        }

                        for (int j = 0; j < collection.Count; j++)
                        {
                            var detail = vehicleRegisterPODetailModelDAO.Where(dt => dt.VehicleRegisterMobileId == collection[j].VehicleRegisterMobileId)
                                                                        .ToList();

                            var driverid = driverInfo.Where(d => d.DriverCardNo == collection[j].DriverIdCard)
                                                            .FirstOrDefault();

                            var poline = polines.Where(ln => ln.Ponumber == collection[j].SoDonHang)
                                                        .ToList();
                            var vehicle = listVehicle.Where(v => v.VehicleNumber.Replace("-", "").Replace(".", "").Replace(" ", "").ToUpper() == collection[j].VehicleNumber.Replace("-", "").Replace(".", "").Replace(" ", "").ToUpper()).FirstOrDefault();
                            var weightAllowed = vehicle.TrongLuongDangKiem - vehicle.VehicleWeight;
                            var isQuaTai = ((collection[j].TrongLuongGiaoDuKien - weightAllowed) > 0) ? true : false;
                            ret.Add(new VehicleMobileResponse { Item = collection[j], Detail = detail, Driver = driverid, Polines = poline, IsQuaTai = isQuaTai, WeightAllowed = weightAllowed });
                        }
                    }          
                }
                return ret;
            }
        }

        public ActionMessage Delete(Guid id)
        {
            var ret = new ActionMessage();
            // WEB
            var registerInfoWEB = VehicleRegisterMobileModelDAO.GetInstance().GetList()
                                                            .Where(v => v.VehicleRegisterMobileId == id)
                                                            .Where(v => v.AllowEdit == true)
                                                            .FirstOrDefault();
            DumpObject("USER DELETE VEHICLEREGISTER: " + id);

            // Kiểm tra nếu xe chưa vô bàn cân thì mới được xoá
            switch (registerInfoWEB.CompanyCode)
            {
                case "3000":
                    var registerInfo3 = VAS_3000Datalayer.GetInstance().GetList()
                                                                .Where(v => v.VehicleRegisterMobileId == id)
                                                                .Where(v => v.AllowEdit == true)
                                                                .FirstOrDefault();

                    if (registerInfo3 != null)
                    {

                        var detail = VehicleRegisterPODetailModelDAO.GetInstance().GetList()
                                                                    .Where(dt => dt.VehicleRegisterMobileId == id).ToList();

                        if (VehicleRegisterMobileModelDAO.GetInstance().DeleteOne(registerInfoWEB) > 0)
                        {
                            //xóa trên bảng pmc
                            VAS_3000Datalayer.GetInstance().DeleteVehicleResigter3000(registerInfoWEB);
                            foreach (var item in detail)
                            {
                                VehicleRegisterPODetailModelDAO.GetInstance().DeleteOne(item);
                                VAS_3000Datalayer.GetInstance().DeleteVehicleDetail3000(item);
                            }
                            ret.isSuccess = true;
                            ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Xóa thông tin đăng ký thành công" };
                        }
                        else
                        {
                            ret.isSuccess = false;
                            ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Xóa không thành công" };
                        }
                    }
                    else
                    {
                        ret.isSuccess = false;
                        ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Không tìm thấy thông tin! Hoặc xe đã vào bãi" };
                    }
                    break;
                case "4000":
                    var registerInfo = VAS_4000Datalayer.GetInstance().GetList()
                                                                .Where(v => v.VehicleRegisterMobileId == id)
                                                                .Where(v => v.AllowEdit == true)
                                                                .FirstOrDefault();

                    if (registerInfo != null)
                    {

                        var detail = VehicleRegisterPODetailModelDAO.GetInstance().GetList()
                                                                    .Where(dt => dt.VehicleRegisterMobileId == id).ToList();

                        if (VehicleRegisterMobileModelDAO.GetInstance().DeleteOne(registerInfoWEB) > 0)
                        {
                            //xóa trên bảng pmc
                            VAS_4000Datalayer.GetInstance().DeleteVehicleResigter4000(registerInfoWEB);
                            foreach (var item in detail)
                            {
                                VehicleRegisterPODetailModelDAO.GetInstance().DeleteOne(item);
                                VAS_4000Datalayer.GetInstance().DeleteVehicleDetail4000(item);
                            }
                            ret.isSuccess = true;
                            ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Xóa thông tin đăng ký thành công" };
                        }
                        else
                        {
                            ret.isSuccess = false;
                            ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Xóa không thành công" };
                        }
                    }
                    else
                    {
                        ret.isSuccess = false;
                        ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Không tìm thấy thông tin! Hoặc xe đã vào bãi" };
                    }
                    break;
                case "6000":
                    var registerInfo6 = VAS_6000Datalayer.GetInstance().GetList()
                                                                .Where(v => v.VehicleRegisterMobileId == id)
                                                                .Where(v => v.AllowEdit == true)
                                                                .FirstOrDefault();


                    if (registerInfo6 != null)
                    {

                        var detail = VehicleRegisterPODetailModelDAO.GetInstance().GetList()
                                                                    .Where(dt => dt.VehicleRegisterMobileId == id).ToList();

                        if (VehicleRegisterMobileModelDAO.GetInstance().DeleteOne(registerInfoWEB) > 0)
                        {
                            //xóa trên bảng pmc
                            VAS_6000Datalayer.GetInstance().DeleteVehicleResigter6000(registerInfoWEB);
                            foreach (var item in detail)
                            {
                                VehicleRegisterPODetailModelDAO.GetInstance().DeleteOne(item);
                                VAS_6000Datalayer.GetInstance().DeleteVehicleDetail6000(item);
                            }
                            ret.isSuccess = true;
                            ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Xóa thông tin đăng ký thành công" };
                        }
                        else
                        {
                            ret.isSuccess = false;
                            ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Xóa không thành công" };
                        }
                    }
                    else
                    {
                        ret.isSuccess = false;
                        ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Không tìm thấy thông tin! Hoặc xe đã vào bãi" };
                    }
                    break;
                default:
                    ret.isSuccess = false;
                    ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Xóa không thành công" };
                    break;

            }
            return ret;
        }


        public ActionMessage DeleteItems(Guid[] items, string username)
        {
            var ret = new ActionMessage();

            // Lấy danh sách các khai báo
            var lstReg = VehicleRegisterMobileModelDAO.GetInstance().GetList()
                                                            .Where(v => items.Contains(v.VehicleRegisterMobileId))
                                                            .Where(v => v.AllowEdit == true)
                                                            .ToList();
            foreach (var item in lstReg)
            {
                DumpObject("USER DELETE VEHICLEREGISTER: " + item.VehicleRegisterMobileId);

                // Kiểm tra nếu xe chưa vô bàn cân thì mới được xoá
                switch (item.CompanyCode)
                {
                    case "3000":
                        var registerInfo3 = VAS_3000Datalayer.GetInstance().GetList()
                                                                    .Where(v => v.VehicleRegisterMobileId == item.VehicleRegisterMobileId)
                                                                    .Where(v => v.AllowEdit == true)
                                                                    .FirstOrDefault();

                        if (registerInfo3 != null)
                        {

                            var detail = VehicleRegisterPODetailModelDAO.GetInstance().GetList()
                                                                        .Where(dt => dt.VehicleRegisterMobileId == item.VehicleRegisterMobileId).ToList();

                            if (VehicleRegisterMobileModelDAO.GetInstance().DeleteOne(item) > 0)
                            {
                                //xóa trên bảng pmc
                                VAS_3000Datalayer.GetInstance().DeleteVehicleResigter3000(item);
                                foreach (var regDetail in detail)
                                {
                                    VehicleRegisterPODetailModelDAO.GetInstance().DeleteOne(regDetail);
                                    VAS_3000Datalayer.GetInstance().DeleteVehicleDetail3000(regDetail);
                                }

                            }
                            else
                            {
                                ret.isSuccess = false;
                                ret.err = new ErorrMssage { msgCode = "4xx", msgString = $"Xóa không thành công xe {registerInfo3.VehicleNumber}" };
                                return ret;
                            }
                        }
                        break;
                    case "4000":
                        var registerInfo = VAS_4000Datalayer.GetInstance().GetList()
                                                                    .Where(v => v.VehicleRegisterMobileId == item.VehicleRegisterMobileId)
                                                                    .Where(v => v.AllowEdit == true)
                                                                    .FirstOrDefault();

                        if (registerInfo != null)
                        {

                            var detail = VehicleRegisterPODetailModelDAO.GetInstance().GetList()
                                                                        .Where(dt => dt.VehicleRegisterMobileId == item.VehicleRegisterMobileId).ToList();

                            if (VehicleRegisterMobileModelDAO.GetInstance().DeleteOne(item) > 0)
                            {
                                //xóa trên bảng pmc
                                VAS_4000Datalayer.GetInstance().DeleteVehicleResigter4000(item);
                                foreach (var regDetail in detail)
                                {
                                    VehicleRegisterPODetailModelDAO.GetInstance().DeleteOne(regDetail);
                                    VAS_4000Datalayer.GetInstance().DeleteVehicleDetail4000(regDetail);
                                }
                            }
                            else
                            {
                                ret.isSuccess = false;
                                ret.err = new ErorrMssage { msgCode = "4xx", msgString = $"Xóa không thành công xe {registerInfo.VehicleNumber}" };
                                return ret;
                            }
                        }
                        break;
                    case "6000":
                        var registerInfo6 = VAS_6000Datalayer.GetInstance().GetList()
                                                                    .Where(v => v.VehicleRegisterMobileId == item.VehicleRegisterMobileId)
                                                                    .Where(v => v.AllowEdit == true)
                                                                    .FirstOrDefault();


                        if (registerInfo6 != null)
                        {

                            var detail = VehicleRegisterPODetailModelDAO.GetInstance().GetList()
                                                                        .Where(dt => dt.VehicleRegisterMobileId == item.VehicleRegisterMobileId).ToList();

                            if (VehicleRegisterMobileModelDAO.GetInstance().DeleteOne(item) > 0)
                            {
                                //xóa trên bảng pmc
                                VAS_6000Datalayer.GetInstance().DeleteVehicleResigter6000(item);
                                foreach (var regDetail in detail)
                                {
                                    VehicleRegisterPODetailModelDAO.GetInstance().DeleteOne(regDetail);
                                    VAS_6000Datalayer.GetInstance().DeleteVehicleDetail6000(regDetail);
                                }
                            }
                            else
                            {
                                ret.isSuccess = false;
                                ret.err = new ErorrMssage { msgCode = "4xx", msgString = $"Xóa không thành công xe {registerInfo6.VehicleNumber}" };
                                return ret;
                            }
                        }
                        break;
                }
            }

            ret.isSuccess = true;
            ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Xóa thông tin thành công!" };
            return ret;
        }
        public ActionMessage Update(VehicleUpdateModel item, Guid id, string userId)
        {
            var ret = new ActionMessage();
            using (var _context = new Web_BookingTransContext())
            {
                //tìm user
                var user = UserModelDAO.GetInstance().GetList().Where(e => e.Username == userId).FirstOrDefault();

                var vehicle = VehicleRegisterMobileModelDAO.GetInstance().GetList()
                                                            .Where(v => v.VehicleRegisterMobileId == id && v.AllowEdit == true).FirstOrDefault();


                // Lấy CompanyCode từ poNumber
                var companyCode = vehicle.CompanyCode;

                // Không tìm thấy xe thoả dk
                if (vehicle == null)
                {
                    ret.isSuccess = false;
                    ret.id = 0;
                    ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Không tìm thấy thông tin xe!" };
                    return ret;
                }


                // Kiểm tra xem phía pmc xe đã vào chưa

                var isAllowEdit = CheckVehicleRegAllowEdit(vehicle);
                if (isAllowEdit == false)
                {
                    ret.isSuccess = false;
                    ret.id = 0;
                    ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Xe đã vào bãi, không được thay đổi!" };
                    return ret;
                }

                //tổng số của đơn hàng po
                decimal? poqantity = 0m;
                decimal? poDaNhap = 0m;
                decimal? totalRegisted = 0m;

                if (user.IsService == true)
                {
                    var lstVehicle = VehicleRegisterMobileModelDAO.GetInstance().GetList()
                        .Where(e => e.Dvvccode == vehicle.Dvvccode && e.SoDonHang == vehicle.SoDonHang)
                        .ToList();

                    // check xem nếu điều phối theo cont thì khôgn cần xét trọng lượng đơn hàng
                    bool? isDieuPhoiCont = _context.OrderMapping
                        .Where(e => e.OrderNumber == vehicle.SoDonHang && e.ServiceId == user.Memberof)
                        .Select(e => e.IsCont)
                        .FirstOrDefault();
                    // Điều phối theo cont thì trọng lượng bằng 0 nên lấy theo trọng lượng của đơn hàng
                    if (isDieuPhoiCont == true)
                    {
                        poqantity = PoMasterModelDAO.GetInstance().GetList()
                            .Where(e => e.Ponumber == vehicle.SoDonHang)
                            .Select(e => e.QtyTotal).FirstOrDefault();
                    }
                    else
                    {
                        //tổng số lượng đơn hàng
                        poqantity = _context.OrderMapping
                            .Where(e => e.OrderNumber == vehicle.SoDonHang && e.ServiceId == user.Memberof)
                            .Select(e => e.SoLuong)
                            .FirstOrDefault();
                    }
                    //Tìm CnnStr để lấy số lượng đã nhập
                    //Lê Hoàng Long
                    string CnnString = CompanyService.GetInstance().GetConnStr(companyCode);

                    //số lượng đã nhập
                    poDaNhap = GetDataFromFunction.GetInstance()
                                                        .GetSLDaNhapTuPONumber(vehicle.SoDonHang, cnt: CnnString);
                    //số lượng po đã đăng ký
                    totalRegisted = lstVehicle.Where(v => v.TrongLuongGiaoThucTe == null || v.TrongLuongGiaoThucTe == 0)
                                                        .Sum(v => v.TrongLuongGiaoDuKien);
                }
                else
                {
                    //tổng số của đơn hàng po
                    poqantity = PoMasterModelDAO.GetInstance().GetList()
                                                        .Where(p => p.Ponumber == vehicle.SoDonHang)
                                                        .Select(p => p.QtyTotal)
                                                        .FirstOrDefault();

                    //Tìm CnnStr để lấy số lượng đã nhập
                    //Lê Hoàng Long
                    string CnnString = CompanyService.GetInstance().GetConnStr(companyCode);


                    //số lượng đã nhập
                    poDaNhap = GetDataFromFunction.GetInstance()
                                                        .GetSLDaNhapTuPONumber(vehicle.SoDonHang, cnt: CnnString);
                    //số lượng po đã đăng ký
                    totalRegisted = VehicleRegisterMobileModelDAO.GetInstance().GetList()
                                                        .Where(v => v.SoDonHang == vehicle.SoDonHang)
                                                        .Where(v => v.TrongLuongGiaoThucTe == null || v.TrongLuongGiaoThucTe == 0)
                                                        .Sum(v => v.TrongLuongGiaoDuKien);
                }

                //tính cho phép còn lại
                if (poqantity != 0)
                {
                    var accept = (poqantity / 100) * 10;
                    var chophep = (poqantity + accept) - (poDaNhap + totalRegisted);

                    if (item.TrongLuong > chophep)
                    {
                        ret.isSuccess = false;
                        ret.id = 0;
                        ret.err = new ErorrMssage { msgCode = "4xx", msgString = string.Format("Vượt quá trọng lượng đơn hàng, trọng lượng còn lại {0:0.##} kg", decimal.Round((decimal)chophep, 0).ToString(specifier, culture)) };
                        return ret;
                    }
                }
                else
                {
                    ret.isSuccess = false;
                    ret.id = 0;
                    ret.err = new ErorrMssage { msgCode = "4xx", msgString = string.Format("Không lấy được tổng số lượng đơn hàng!") };
                    return ret;
                }

                //nếu không vượt quá cho phép thì cập nhật lại
                // if (vehicle != null)
                // {
                var driver = DriverRegisterDAO.GetInstance().GetList().Where(d => d.DriverId == item.DriverId).FirstOrDefault();


               
                if (vehicle.SoDonHang != item.OrderNumber)
                {
                    companyCode = PoMasterModelDAO.GetInstance().GetList()
                                                    .Where(p => p.Ponumber == item.OrderNumber)
                                                    .Select(p => p.CompanyCode)
                                                    .FirstOrDefault();
                }

                if (driver != null)
                {
                    vehicle.DriverIdCard = driver.DriverCardNo;
                    vehicle.DriverName = driver.DriverName;
                    vehicle.Assets = item.Assets;
                    vehicle.TrongLuongGiaoDuKien = item.TrongLuong;
                    vehicle.SoDonHang = item.OrderNumber;
                    vehicle.CompanyCode = companyCode; // Cập nhật lại CompanyCode theo POMaster
                    //vehicle.IsActive = item.IsActive;

                    //cập nhật vehicle
                    var ids = VehicleRegisterMobileModelDAO.GetInstance().UpdateOne(vehicle);

                    if (ids > 0)
                    {
                        //list po cũ
                        var detail = VehicleRegisterPODetailModelDAO.GetInstance().GetList()
                                                                .Where(dt => dt.VehicleRegisterMobileId == id).ToList();
                        foreach (var items in detail)
                        {
                            VehicleRegisterPODetailModelDAO.GetInstance().DeleteOne(items);
                            DeleteVehicleRegisterDetailOnPMC(items, companyCode);
                            // VAS_4000Datalayer.GetInstance().DeleteVehicleDetail4000(items);
                        }
                        //lấy chi tiết từ poline master
                        var poln = PoLinesModelDAO.GetInstance().GetList().Where(ln => ln.Ponumber == vehicle.SoDonHang).ToList();
                        //tạo mới
                        foreach (var poline in item.VatTuUpdate)
                        {
                            var ln = new WebModels.VehicleRegisterPodetailModel();
                            ln.VehicleRegisterPodetailId = Guid.NewGuid();
                            ln.VehicleRegisterMobileId = vehicle.VehicleRegisterMobileId;
                            ln.Ponumber = vehicle.SoDonHang;
                            ln.Poline = poln.Where(l => l.ProductCode == poline)
                                            .Select(l => l.Poline)
                                            .FirstOrDefault();
                            ln.ProductCode = poline;
                            ln.ProductName = poln.Where(l => l.ProductCode == poline)
                                            .Select(l => l.ProductName)
                                            .FirstOrDefault();
                            //
                            VehicleRegisterPODetailModelDAO.GetInstance().InsertOne(ln);
                            //insert vào bảng pmc
                            InsertVehicleRegisterDetailToPMC(ln, companyCode);
                            // VAS_4000Datalayer.GetInstance().InsertVehicleDetail4000(ln);
                        }
                        UpdateVehicleRegisterToPMC(vehicle, companyCode);
                        // VAS_4000Datalayer.GetInstance().UpdateVehicleResigter4000(vehicle);

                        ret.isSuccess = true;
                        ret.id = ids;
                        ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Cập nhật thành công!" };
                    }
                    else
                    {
                        ret.isSuccess = false;
                        ret.id = 0;
                        ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Cập nhật thất bại!" };
                    }
                }
                else
                {
                    ret.isSuccess = false;
                    ret.id = 0;
                    ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Không tìm thấy thông tin tài xế!" };
                }
                // }
                // else
                // {
                //     ret.isSuccess = false;
                //     ret.id = 0;
                //     ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Không tìm thấy thông tin xe!" };
                // }
            }
            return ret;
        }


        public ActionMessage ActiveVehicleRegis(Guid id, string userId)
        {
            var ret = new ActionMessage();
            // xe khai báo PO thường
            var vehicle = new WebModels.VehicleRegisterMobileModel();
            using (var _context = new Web_BookingTransContext())
            {
                vehicle = _context.VehicleRegisterMobileModel.Where(e => e.VehicleRegisterMobileId == id).FirstOrDefault();
            }
            if (vehicle == null)
            {
                ret.isSuccess = false;
                ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Không tìm thấy thông tin đăng ký" };
                return ret;
            }

            using (var _context = new Web_BookingTransContext())
            {
                //list các xe
                var lstDiferenceOrder = _context.VehicleRegisterMobileModel
                .Where(e => e.VehicleNumber == vehicle.VehicleNumber)//cùng biển số
                                                                     //.Where(e => e.Dvvccode == vehicle.Dvvccode)//cùng đơn vị vận chuyển
                .Where(e => e.SoDonHang != vehicle.SoDonHang)//khác đơn hàng
                .Where(e => e.ThoiGianToiDuKien >= DateTime.Now)//còn hạn giao
                .Where(e => e.AllowEdit == true)//chưa vào cân
                .Where(e => e.CompanyCode == vehicle.CompanyCode) // Xét trên cùng plant
                .ToList();
                /*
                 * TH 1: xe đó đăng ký chở nhiều PO cùng lúc
                 * => chỉ active 1 dòng dăng ký duy nhất và inactive toàn bộ các dòng còn lại
                 * TH 2: xe đó chỉ đang đăng ký cho 1 PO nhưng đang bị inactive
                 * => Kích hoạt toàn bộ những dòng đăng ký còn lại
                 * */
                if (lstDiferenceOrder.Count > 0 && lstDiferenceOrder != null)
                {
                    // TH1
                    for (int i = 0; i < lstDiferenceOrder.Count; i++)
                    {
                        //nếu đang active thì deactive
                        if (lstDiferenceOrder[i].IsActive == true)
                        {
                            lstDiferenceOrder[i].IsActive = false;
                        }
                        _context.Update(lstDiferenceOrder[i]);
                        _context.SaveChanges();
                        //thêm vào bên pmc
                        UpdateVehicleRegisterToPMC(lstDiferenceOrder[i], vehicle.CompanyCode);
                        // VAS_4000Datalayer.GetInstance().UpdateVehicleResigter4000(lstDiferenceOrder[i]);
                    }

                    // inactive toàn bộ đăng ký khác của cùng po đó
                    var lstSameOrder = _context.VehicleRegisterMobileModel
                    .Where(e => e.VehicleNumber == vehicle.VehicleNumber)//cùng biển số
                    .Where(e => e.Dvvccode == vehicle.Dvvccode)//cùng đơn vị vận chuyển
                    .Where(e => e.SoDonHang == vehicle.SoDonHang)//cùng đơn hàng
                    .Where(e => e.ThoiGianToiDuKien >= DateTime.Now)//còn hạn giao
                    .Where(e => e.AllowEdit == true)//chưa vào cân
                    .Where(e => e.VehicleRegisterMobileId != vehicle.VehicleRegisterMobileId)//không bao gồm bản thân
                    .ToList();
                    //nếu được đăng ký nhiều lần cho cái po đó
                    if (lstSameOrder.Count > 0 && lstSameOrder != null)
                    {
                        for (int i = 0; i < lstSameOrder.Count; i++)
                        {
                            if (lstSameOrder[i].IsActive == true)
                            {
                                lstSameOrder[i].IsActive = false;
                            }
                            _context.Update(lstSameOrder[i]);
                            _context.SaveChanges();
                            //thêm vào bên pmc
                            UpdateVehicleRegisterToPMC(lstSameOrder[i], vehicle.CompanyCode);
                            // VAS_4000Datalayer.GetInstance().UpdateVehicleResigter4000(lstSameOrder[i]);
                        }
                    }
                }
                else
                {
                    // TH2
                    // Trường hợp xe đó chỉ đang đăng ký cho 1 po => active cho toàn bộ những xe giống y chan còn lại
                    var lstSameRegis = _context.VehicleRegisterMobileModel
                    .Where(e => e.VehicleNumber == vehicle.VehicleNumber)//cùng biển số
                    .Where(e => e.Dvvccode == vehicle.Dvvccode)//cùng đơn vị vận chuyển
                    .Where(e => e.SoDonHang == vehicle.SoDonHang)//cùng đơn hàng
                    .Where(e => e.ThoiGianToiDuKien >= DateTime.Now)//còn hạn giao
                    .Where(e => e.AllowEdit == true)//chưa vào cân
                    .Where(e => e.VehicleRegisterMobileId != vehicle.VehicleRegisterMobileId)//không bao gồm bản thân
                    .ToList();

                    //nếu được đăng ký nhiều lần cho cái po đó
                    for (int i = 0; i < lstSameRegis.Count; i++)
                    {
                        if (lstSameRegis[i].IsActive == false)
                        {
                            lstSameRegis[i].IsActive = true;
                        }
                        _context.Update(lstSameRegis[i]);
                        _context.SaveChanges();
                        //thêm vào bên pmc
                        UpdateVehicleRegisterToPMC(lstSameRegis[i], vehicle.CompanyCode);
                        // VAS_4000Datalayer.GetInstance().UpdateVehicleResigter4000(lstSameRegis[i]);
                    }
                }
                //thay đổi trạng thái kích hoạt của xe
                vehicle.IsActive = true;
                vehicle.UserActiveId = _context.UserModel.Where(e => e.Username == userId).Select(e => e.Userid).FirstOrDefault();
                _context.Update(vehicle);
                _context.SaveChanges();
                UpdateVehicleRegisterToPMC(vehicle, vehicle.CompanyCode);
                // VAS_4000Datalayer.GetInstance().UpdateVehicleResigter4000(vehicle);
            }
            ret.isSuccess = true;
            ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Kích hoạt thành công" };




            // xe khai báo PO chung tàu và có plant là 3000
            return ret;
        }




        // Insert thông tin khai báo xuống PMC
        private void InsertVehicleResigterToPMC(WebModels.VehicleRegisterMobileModel vehicleInfo, string companyCode)
        {
            switch (companyCode)
            {
                case "3000":
                    VAS_3000Datalayer.GetInstance().InsertVehicleResigter3000(vehicleInfo);
                    break;
                case "4000":
                    VAS_4000Datalayer.GetInstance().InsertVehicleResigter4000(vehicleInfo);
                    break;
                case "6000":
                    VAS_6000Datalayer.GetInstance().InsertVehicleResigter6000(vehicleInfo);
                    break;
            }

        }

        // Insert chi tiết khao báo xuống PMC
        private void InsertVehicleRegisterDetailToPMC(WebModels.VehicleRegisterPodetailModel vehicleInfo, string companyCode)
        {
            switch (companyCode)
            {
                case "3000":
                    VAS_3000Datalayer.GetInstance().InsertVehicleDetail3000(vehicleInfo);
                    break;
                case "4000":
                    VAS_4000Datalayer.GetInstance().InsertVehicleDetail4000(vehicleInfo);
                    break;
                case "6000":
                    VAS_6000Datalayer.GetInstance().InsertVehicleDetail6000(vehicleInfo);
                    break;
            }

        }

        // Cập nhật thôgn tin khai báo giao nhậnn cho đúng plant
        private void UpdateVehicleRegisterToPMC(WebModels.VehicleRegisterMobileModel vehicleRegisterMobileModel, string companyCode)
        {
            switch (companyCode)
            {
                case "3000":
                    VAS_3000Datalayer.GetInstance().UpdateVehicleResigter3000(vehicleRegisterMobileModel);
                    break;
                case "4000":
                    VAS_4000Datalayer.GetInstance().UpdateVehicleResigter4000(vehicleRegisterMobileModel);
                    break;
                case "6000":
                    VAS_6000Datalayer.GetInstance().UpdateVehicleResigter6000(vehicleRegisterMobileModel);
                    break;
            }
        }

        private Boolean CheckVehicleRegAllowEdit(WebModels.VehicleRegisterMobileModel vehicleRegisterMobileModel)
        {
            string companyCode = vehicleRegisterMobileModel.CompanyCode;
            if (companyCode == Constants.PlantConstants.P3000)
            {
                using (var _context = new Vas_3000Context())
                {
                    var vehicleReg = _context.VehicleRegisterMobileModel.Where(v => v.VehicleRegisterMobileId == vehicleRegisterMobileModel.VehicleRegisterMobileId && v.AllowEdit == true).FirstOrDefault();
                    if (vehicleReg != null)
                    {
                        return true;
                    }
                }

            }
            else if (companyCode == Constants.PlantConstants.P4000)
            {
                using (var _context = new Vas_4000Context())
                {
                    var vehicleReg = _context.VehicleRegisterMobileModel.Where(v => v.VehicleRegisterMobileId == vehicleRegisterMobileModel.VehicleRegisterMobileId && v.AllowEdit == true).FirstOrDefault();
                    if (vehicleReg != null)
                    {
                        return true;
                    }
                }
            }
            else if (companyCode == Constants.PlantConstants.P6000)
            {
                using (var _context = new VAS6000.Vas_6000Context())
                {
                    var vehicleReg = _context.VehicleRegisterMobileModel.Where(v => v.VehicleRegisterMobileId == vehicleRegisterMobileModel.VehicleRegisterMobileId && v.AllowEdit == true).FirstOrDefault();
                    if (vehicleReg != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        private void DeleteVehicleRegisterDetailOnPMC(WebModels.VehicleRegisterPodetailModel items, string companyCode)
        {
            switch (companyCode)
            {
                case "3000":
                    VAS_3000Datalayer.GetInstance().DeleteVehicleDetail3000(items);
                    break;
                case "4000":
                    VAS_4000Datalayer.GetInstance().DeleteVehicleDetail4000(items);
                    break;
                case "6000":
                    VAS_6000Datalayer.GetInstance().DeleteVehicleDetail6000(items);
                    break;
            }
        }
    }
}