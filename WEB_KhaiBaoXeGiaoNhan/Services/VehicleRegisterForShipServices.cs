using AutoMapper;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using WEB_KhaiBaoXeGiaoNhan.ADO;
using WEB_KhaiBaoXeGiaoNhan.Datalayers;
using WEB_KhaiBaoXeGiaoNhan.Models;
using WEB_KhaiBaoXeGiaoNhan.Responses;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Services
{
    public class VehicleRegisterForShipServices : BaseService<VehicleRegisterForShipServices>
    {
        public ActionMessage RegisterPO(RegisterForShipModel model, string username)
        {
            var ret = new ActionMessage();
            if (model == null)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "4xx";
                ret.err.msgString = "Dữ liệu vào sai";
                return ret;
            }

            if (model.ListVehicle.Count == 0)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "4xx";
                ret.err.msgString = "Danh sách xe trống";
                return ret;
            }


            

            var user = UserModelDAO.GetInstance().GetList()
                                                       .Where(u => u.Username == username)
                                                       .Where(u => u.IsService == true)
                                                       .FirstOrDefault();
            if(user == null)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "4xx";
                ret.err.msgString = "Không phải là ĐVVC";
                return ret;
            }
            // Kiểm tra danh sách xe gưi rleen có xe nào đã khai báo trước đó chưa, có thì thoogn báo ngay luôn
            var lstVehicleNumber = model.ListVehicle.Select(u => u.VehicleNumber).ToList();
            var lstVehiclenumberDistinct = lstVehicleNumber.Distinct();
            // Check lặp biển số khi gửi lên
            if(lstVehicleNumber.Count != lstVehiclenumberDistinct.Count())
            {
                ret.isSuccess = false;
                ret.err.msgCode = "4xx";
                ret.err.msgString = "Biển số xe bị lặp lại, vui lòng kiểm tra danh sách khai báo";
                return ret;
            }

            if (String.IsNullOrEmpty(model.ProductCode))
            {
                ret.isSuccess = false;
                ret.err.msgCode = "4xx";
                ret.err.msgString = "Chưa chọn mặt hàng";
                return ret;
            }

            using (var _context = new Web_BookingTransContext())
            {
                var lstXeDaKhaiBao = _context.RegisterForShip.Where(e => lstVehicleNumber.Contains(e.VehicleNumber))
                    .Where(e => e.Status == true)
                    .Select(e => e.VehicleNumber)
                    .ToList();

                if(lstXeDaKhaiBao != null && lstXeDaKhaiBao.Count() != 0)
                {
                    var errorString = "";
                    foreach (var item in lstXeDaKhaiBao)
                    {
                        errorString = errorString + " ," + item;
                    }
                    ret.isSuccess = false;
                    ret.err.msgCode = "4xx";
                    ret.err.msgString = $"Xe {errorString} đã được khai báo, vui lòng xem lại thông tin";
                    return ret;
                }
            }


            var provider = ProviderModelDAO.GetInstance().GetList()
                                                    .Where(p => p.ProviderId == user.Memberof)
                                                    .FirstOrDefault();


            var pomaster = PoMasterModelDAO.GetInstance().GetList()
                                                .Where(p => p.Ponumber == model.OrderNumber)
                                                .FirstOrDefault();

            string productName = default;

            using (var _context = new Web_BookingTransContext())
            {
                productName = _context.PolineModel.Where(e => e.Ponumber == model.OrderNumber)
                    .Where(e => e.ProductCode == model.ProductCode)
                    .Select(e => e.ProductName)
                    .FirstOrDefault();
            }

            var companyCode = pomaster.CompanyCode; // lấy companyCode từ poMaster gắn vào đăng ký


            
            foreach (var item in model.ListVehicle)
            {

                var taiXeCa1 = DriverRegisterDAO.GetInstance().GetList()
                                                       .Where(d => d.DriverId == item.DriverID1)
                                                       .FirstOrDefault();

                var taiXeCa2 = DriverRegisterDAO.GetInstance().GetList()
                                                       .Where(d => d.DriverId == item.DriverID2)
                                                       .FirstOrDefault();

                OrderMapping mapingInfo = new OrderMapping();
                using (var _context = new Web_BookingTransContext())
                {
                    mapingInfo = _context.OrderMapping.Where(e => e.ServiceId == user.Memberof)
                                                .Where(e => e.OrderNumber == model.OrderNumber)
                                                .FirstOrDefault();
                }


                // 1 xe mà khai nhiều PO cùng lúc status = 1 thì khogon cho lưu
                var vehicleInfo = new WebModels.RegisterForShip
                {
                    PoNumber = model.OrderNumber,
                    Dvvc = provider.ProviderName,
                    DvvcCode = provider.ProviderCode,
                    VehicleNumber = item.VehicleNumber,
                    DriverName1 = taiXeCa1.DriverName,
                    DriverIdCard1 = taiXeCa1.DriverCardNo,
                    DriverName2 = taiXeCa2.DriverName,
                    DriverIdCard2 = taiXeCa2.DriverCardNo,
                    Romooc = item.Romooc,
                    ShipNumber = mapingInfo.ShipNumber,
                    CompanyCode = companyCode,
                    CreatedAt = DateTime.Now,
                    CreatedBy = user.Userid,
                    StartAt = model.StartDate,
                    Status = true,
                    ProductCode = model.ProductCode,
                    ProductName = productName
                };

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


                DumpObject("KHAI BAO THEO TAU MOI:");
                DumpObject(vehicleInfo);
                // NCC khai báo xe
                ret = NCCKhaiBaoXe(vehicleInfo);


            }

            return ret;
        }

        public ActionMessage DeleteRegisterSingle(int id, string username)
        {
            var ret = new ActionMessage();
            var user = UserModelDAO.GetInstance().GetList()
                                                       .Where(u => u.Username == username)
                                                       .FirstOrDefault();

            // lấy DVVC
            string dvvcCode = default;

            using (var _context = new Web_BookingTransContext())
            {

                dvvcCode = _context.ProviderModel
                    .Where(e => e.ProviderId == user.Memberof)
                    .Select(e => e.ProviderCode)
                    .FirstOrDefault();
            }

            // lấy chi tiết của khai báo
            var vehicleRegis = new RegisterForShip();
            using (var _context = new Web_BookingTransContext())
            {

                vehicleRegis = _context.RegisterForShip
                    .Where(e => e.Id == id)
                    .Where(e => e.DvvcCode == dvvcCode)
                    .FirstOrDefault();
            }
            if (vehicleRegis != null)
            {
                DumpObject("DELETE KHAI BAO THEO TAU:");
                DumpObject(vehicleRegis);
                var result = RegisterForShipDAO.GetInstance().DeleteOne(vehicleRegis);
                if (result == 1)
                {
                    ret.isSuccess = true;
                    ret.err.msgCode = "2xx";
                    ret.err.msgString = "Xóa thành công";
                    return ret;
                }
            }

            ret.isSuccess = false;
            ret.err.msgCode = "4xx";
            ret.err.msgString = "Không thành công";

            return ret;
        }

        public List<VehicleRegisForShipResponse> GetList(string username, string shipNumber, string orderNumber, string productCode)
        {
            List<RegisterForShip> lstVehicle = new List<RegisterForShip>();

            var user = UserModelDAO.GetInstance().GetList()
                                                       .Where(u => u.Username == username)
                                                       .FirstOrDefault();


            var provider = ProviderModelDAO.GetInstance().GetList()
                                                    .Where(p => p.ProviderId == user.Memberof)
                                                    .FirstOrDefault();


            using (var _context = new Web_BookingTransContext())
            {

                lstVehicle = _context.RegisterForShip
                    .Where(e => e.DvvcCode == provider.ProviderCode)
                    .ToList();
            }

            if (!String.IsNullOrEmpty(shipNumber))
            {
                lstVehicle = lstVehicle.Where(e => e.ShipNumber == shipNumber).ToList();
            }
            if (!String.IsNullOrEmpty(orderNumber))
            {
                lstVehicle = lstVehicle.Where(e => e.PoNumber == orderNumber).ToList();
            }

            if (!String.IsNullOrEmpty(productCode))
            {
                lstVehicle = lstVehicle.Where(e => e.ProductCode == productCode).ToList();
            }


            var lstDriverId = lstVehicle.Select(e => e.DriverIdCard1).ToList();
            lstDriverId.AddRange(lstVehicle.Select(e => e.DriverIdCard2).ToList());

            var lstDriver = new List<DriverRegister>();
            // lấy danh sách tài xế
            using (var _context = new Web_BookingTransContext())
            {

                lstDriver = _context.DriverRegister
                    .Where(e => lstDriverId.Contains(e.DriverCardNo))
                    .ToList();

            }



            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RegisterForShip, VehicleRegisForShipResponse>();

            });

            var mapper = new Mapper(configuration);
            List<VehicleRegisForShipResponse> response = mapper.Map<List<VehicleRegisForShipResponse>>(lstVehicle);

            foreach (var responseItem in response)
            {
                responseItem.DriverId1 = lstDriver.Where(e => e.DriverCardNo == responseItem.DriverIdCard1)
                    .Select(e => e.DriverId).FirstOrDefault();
                responseItem.DriverId2 = lstDriver.Where(e => e.DriverCardNo == responseItem.DriverIdCard2)
                    .Select(e => e.DriverId).FirstOrDefault();
            }


            return response;
        }

        public ActionMessage UpdateRegister(RegisterForShipModel model, string username)
        {
            var ret = new ActionMessage();
            if (model == null)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "4xx";
                ret.err.msgString = "Không có thông tin";
                return ret;
            }

            if (model.ListVehicle.Count == 0)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "4xx";
                ret.err.msgString = "Danh sách xe trống";
                return ret;
            }

            var user = UserModelDAO.GetInstance().GetList()
                                                       .Where(u => u.Username == username)
                                                       .Where(u => u.IsService == true)
                                                       .FirstOrDefault();
            if(user == null)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "4xx";
                ret.err.msgString = "Không phải là ĐVVC";
                return ret;
            }

            //Check null ProductCode
            if (String.IsNullOrEmpty(model.ProductCode))
            {
                ret.isSuccess = false;
                ret.err.msgCode = "4xx";
                ret.err.msgString = "Thiếu thông tin mặt hàng";
                return ret;
            }

            // Kiểm tra danh sách xe gưi rleen có xe nào đã khai báo trước đó chưa, có thì thoogn báo ngay luôn
            var lstVehicleNumber = model.ListVehicle.Select(u => u.VehicleNumber).ToList();
            var lstVehiclenumberDistinct = lstVehicleNumber.Distinct();
            // Check lặp biển số khi gửi lên
            if (lstVehicleNumber.Count != lstVehiclenumberDistinct.Count())
            {
                ret.isSuccess = false;
                ret.err.msgCode = "4xx";
                ret.err.msgString = "Biển số xe bị lặp lại, vui lòng kiểm tra danh sách khai báo";
                return ret;
            }

            var provider = ProviderModelDAO.GetInstance().GetList()
                                                    .Where(p => p.ProviderId == user.Memberof)
                                                    .FirstOrDefault();


            //số lượng nhập cho phép của po
            var pomaster = PoMasterModelDAO.GetInstance().GetList()
                                                .Where(p => p.Ponumber == model.OrderNumber)
                                                .FirstOrDefault();

            var companyCode = pomaster.CompanyCode; // lấy companyCode từ poMaster gắn vào đăng ký


            // Thêm vào từng xe

            


            var lstId = model.ListVehicle.Where(e => e.Id != null).Select(e => e.Id).ToList();
            var lstDaKhaiBao = new List<RegisterForShip>();
            // Ds đã khai báo
            using (var _context = new Web_BookingTransContext())
            {
                lstDaKhaiBao = _context.RegisterForShip.Where(e => e.PoNumber == model.OrderNumber)
                    .Where(e => e.ProductCode == model.ProductCode)
                    .Where(e=> e.DvvcCode == provider.ProviderCode) // cùng 1 DVVC
                    .ToList();
            }

            var lstToRemove = lstDaKhaiBao.Where(e=> !lstId.Contains(e.Id)).ToList();
            // Xóa danh sách khoogn gửi lên
            foreach (var item in lstToRemove)
            {
                try
                {
                    DumpObject("XOA KHI UPDATE: ");
                    DumpObject(item);
                    RegisterForShipDAO.GetInstance().DeleteOne(item);
                }
                catch (Exception ex)
                {
                    DumpObject(ex.ToString());
                }
            }

            string productName = default;

            using (var _context = new Web_BookingTransContext())
            {
                productName = _context.PolineModel.Where(e => e.Ponumber == model.OrderNumber)
                    .Where(e => e.ProductCode == model.ProductCode)
                    .Select(e => e.ProductName)
                    .FirstOrDefault();
            }


            foreach (var item in model.ListVehicle)
            {

                // Ktra xe đã khai báo trước đó chưa có rồi thì không cho khai báo nữa
                
                using (var _context = new Web_BookingTransContext())
                {
                    var xeTrungBienSo = _context.RegisterForShip.Where(e => e.VehicleNumber == item.VehicleNumber)
                                                .Where(e => e.Status == true)
                                                .FirstOrDefault();
                    // if(xeTrungBienSo != null && xeTrungBienSo.Id)
                }


                var taiXeCa1 = DriverRegisterDAO.GetInstance().GetList()
                                                       .Where(d => d.DriverId == item.DriverID1)
                                                       .FirstOrDefault();

                var taiXeCa2 = DriverRegisterDAO.GetInstance().GetList()
                                                       .Where(d => d.DriverId == item.DriverID2)
                                                       .FirstOrDefault();

                OrderMapping mapingInfo = new OrderMapping();
                using (var _context = new Web_BookingTransContext())
                {
                    mapingInfo = _context.OrderMapping.Where(e => e.ServiceId == user.Memberof)
                                                .Where(e => e.OrderNumber == model.OrderNumber)
                                                .FirstOrDefault();
                }





                var vehicleInfo = new WebModels.RegisterForShip
                {
                    PoNumber = model.OrderNumber,
                    Dvvc = provider.ProviderName,
                    DvvcCode = provider.ProviderCode,
                    VehicleNumber = item.VehicleNumber,
                    DriverName1 = taiXeCa1.DriverName,
                    DriverIdCard1 = taiXeCa1.DriverCardNo,
                    DriverName2 = taiXeCa2.DriverName,
                    DriverIdCard2 = taiXeCa2.DriverCardNo,
                    Romooc = item.Romooc,
                    ShipNumber = mapingInfo.ShipNumber,
                    CompanyCode = companyCode,
                    CreatedAt = DateTime.Now,
                    CreatedBy = user.Userid,
                    StartAt = model.StartDate,
                    Status = true,
                    ProductCode = model.ProductCode,
                    ProductName = productName
                };


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

                if (item.Id != null)
                {
                    // update
                    vehicleInfo.Id = (int)item.Id;
                    using (var _context = new Web_BookingTransContext())
                    {
                        var odlRegister = _context.RegisterForShip.Where(e => e.Id == item.Id)
                                                    .Where(e => e.DvvcCode == provider.ProviderCode)
                                                    .FirstOrDefault();
                        if (odlRegister != null)
                        {
                            vehicleInfo.CreatedAt = odlRegister.CreatedAt;
                            vehicleInfo.UpdatedAt = DateTime.Now;
                            vehicleInfo.CreatedBy = odlRegister.CreatedBy;
                            vehicleInfo.UpdatedBy = user.Userid;
                        }
                        else
                        {
                            // Khoogn đúng id của cùng dvvc
                            ret.isSuccess = false;
                            ret.err.msgCode = "4xx";
                            ret.err.msgString = $"Không thể cập nhật khai báo của DVVC khác";
                            return ret;
                        }
                    }
                    DumpObject("CAP KHAI BAO THEO TAU");
                    DumpObject(vehicleInfo);
                    RegisterForShipDAO.GetInstance().UpdateOne(vehicleInfo);
                }
                else
                {
                    // create
                    using (var _context = new Web_BookingTransContext())
                    {
                        var vehicleDaKhaiBao = _context.RegisterForShip.Where(e => e.VehicleNumber == item.VehicleNumber)
                                                    .Where(e => e.Status == true)
                                                    .FirstOrDefault();
                        if (vehicleDaKhaiBao != null)
                        {
                            ret.isSuccess = false;
                            ret.err.msgCode = "4xx";
                            ret.err.msgString = $"Xe {item.VehicleNumber} đã được khai báo trước đó";
                            return ret;
                        }
                    }


                    DumpObject("TAO MOI KHAI BAO THEO TAU");
                    DumpObject(vehicleInfo);


                    var result = RegisterForShipDAO.GetInstance().InsertOne(vehicleInfo);
                    if (result != 1)
                    {
                        ret.isSuccess = false;
                        ret.err.msgCode = "4xx";
                        ret.err.msgString = $"Không thêm được xe {item.VehicleNumber} ";
                        return ret;
                    }

                }

            }

            ret.isSuccess = true;
            ret.err.msgCode = "2xx";
            ret.err.msgString = "Cập nhật thông tin khai báo thành công";

            return ret;
        }




        private ActionMessage NCCKhaiBaoXe(WebModels.RegisterForShip vehicleInfo)
        {
            var ret = new ActionMessage();
            /*
            List<WebModels.RegisterForShip> lst = new List<WebModels.RegisterForShip>();

            using (var _context = new Web_BookingTransContext())
            {
                lst = _context.RegisterForShip//.Where(e => e.Dvvccode == vehicleInfo.Dvvccode)
                                                     .Where(e => e.Ponumber != vehicleInfo.Ponumber)
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
            */


            var result = RegisterForShipDAO.GetInstance().InsertOne(vehicleInfo);
            if (result == 1)
            {
                ret.isSuccess = true;
                ret.err.msgCode = "2xx";
                ret.err.msgString = "Thêm thành công";
            }


            return ret;
        }

        public ActionMessage DeleteRegister(string ponumber, string productCode, string username)
        {
            var ret = new ActionMessage();

            using (var _context = new Web_BookingTransContext())
            {
                var vehicleDaKhaiBao = _context.RegisterForShip.Where(e => e.PoNumber == ponumber)
                                            .Where(e => e.ProductCode == productCode)
                                            .ToList();
                foreach (var item in vehicleDaKhaiBao)
                {
                    try
                    {
                        var result = RegisterForShipDAO.GetInstance().DeleteOne(item);
                        if (result == 1)
                        {
                            ret.isSuccess = true;
                            ret.err.msgCode = "2xx";
                            ret.err.msgString = "Xóa thành công";
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return ret;

        }
    }
}
