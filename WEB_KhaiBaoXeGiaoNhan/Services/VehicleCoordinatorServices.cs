using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_KhaiBaoXeGiaoNhan.Datalayers;
using WEB_KhaiBaoXeGiaoNhan.Models;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Services
{
    public class VehicleCoordinatorServices : BaseService<VehicleCoordinatorServices>
    {
        public List<ProviderModel> GetVehicle()
        {
            var ret = new List<ProviderModel>();
            //master data
            var lstVehicle = VehicleModelDAO.GetInstance().GetList();
            var lstProvider = ProviderModelDAO.GetInstance().GetList();
            //danh sách mã nhà cung cấp
            var listServicesCode = lstVehicle
                .Where(v => v.VehicleOwner != null)
                .Where(v => v.VehicleOwner.StartsWith("S"))
                .Select(v => v.VehicleOwner.Remove(0, 1))
                .Distinct()
                .ToList();
            //danh sách chi tiết nhà vận chuyển
            var listService = lstProvider.Where(p => listServicesCode.Contains(p.ProviderCode)).ToList();

            foreach (var item in listService)
            {
                var i = new ProviderModel();
                i = item;
                ret.Add(i);
            }
            return ret;
        }

        public ActionMessage MappingPo(MappingModel value)
        {
            var ret = new ActionMessage();

            //danh sách poline
            var lstPoline = PoLinesModelDAO.GetInstance().GetList()
                .Where(ln => ln.Ponumber == value.OrderNumber)
                .Where(ln => ln.DeliveryDate >= DateTime.Now)
                .ToList();
            if (lstPoline == null || lstPoline.Count <= 0)
            {
                ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Số đơn hàng đã hết hiệu lực hoặc không tồn tại" };
                ret.isSuccess = false;
            }
            else
            {
                //danh sách po
                var lstPo = PoMasterModelDAO.GetInstance().GetList()
                    .Where(p => p.Ponumber == value.OrderNumber).FirstOrDefault();

                //
                using (var _context = new Web_BookingTransContext())
                {
                    if (value != null)
                    {
                        //list maping của master
                        var lstMapping = _context.OrderMapping.Where(e => e.MasterId == value.MasterID)
                            .Where(e=> e.OrderNumber == value.OrderNumber);
                        //xóa bỏ cũ
                        //_context.OrderMapping.RemoveRange(lstMapping);
                        //_context.SaveChanges();
                        List<Guid> newMappingId = new List<Guid>();
                        if (value.Services != null || value.Services.Count > 0)
                        {
                            
                            foreach (var item in value.Services)
                            {
                                // cập nhật vào line cũ
                                var mapping = lstMapping.Where(e => e.ServiceId == item.ServicesID).FirstOrDefault();
                                if(mapping != null)
                                {
                                    
                                    mapping.SoLuong = item.Quantity;
                                    mapping.CungDuongCode = item.CungDuongCode;
                                    mapping.IsCont = value.IsCont;
                                    mapping.SoLuongCont = item.SoLuongCont;
                                    mapping.BillNumber = value.BillNumber;
                                    mapping.ShipNumber = value.ShipNumber;
                                    mapping.CreatedTime = DateTime.Now;
                                    if (value.StartDate != null && value.EndDate != null)
                                    {
                                        mapping.StartDate = value.StartDate;
                                        mapping.EndDate = value.EndDate;
                                    }
                                    mapping.Status = true;

                                    using (var trans = _context.Database.BeginTransaction())
                                    {
                                        try
                                        {
                                            _context.OrderMapping.Update(mapping);
                                            _context.SaveChanges();
                                            trans.Commit();
                                        }
                                        catch (Exception e)
                                        {
                                            trans.Rollback();
                                            ret.err = new ErorrMssage { msgCode = "4xx", msgString = e.Message };
                                            ret.isSuccess = false;
                                            return ret;
                                        }
                                    }
                                }
                                else
                                {
                                    // tạo mới
                                    mapping= new OrderMapping();
                                    mapping.MappingId = Guid.NewGuid();
                                    mapping.ServiceId = item.ServicesID;
                                    mapping.MasterId = value.MasterID;
                                    mapping.OrderNumber = value.OrderNumber;
                                    mapping.SoLuong = item.Quantity;
                                    mapping.CungDuongCode = item.CungDuongCode;
                                    mapping.IsCont = value.IsCont;
                                    mapping.SoLuongCont = item.SoLuongCont;
                                    mapping.BillNumber = value.BillNumber;
                                    mapping.ShipNumber = value.ShipNumber;
                                    mapping.CreatedTime = DateTime.Now;
                                    if (value.StartDate != null && value.EndDate != null)
                                    {
                                        mapping.StartDate = value.StartDate;
                                        mapping.EndDate = value.EndDate;
                                    }
                                    mapping.Status = true;

                                    // lưu db
                                    using (var trans = _context.Database.BeginTransaction())
                                    {
                                        try
                                        {
                                            _context.OrderMapping.Add(mapping);
                                            _context.SaveChanges();
                                            trans.Commit();
                                        }
                                        catch (Exception e)
                                        {
                                            trans.Rollback();
                                            ret.err = new ErorrMssage { msgCode = "4xx", msgString = e.Message };
                                            ret.isSuccess = false;
                                            return ret;
                                        }
                                    }
                                }
                                newMappingId.Add(mapping.MappingId);

                            }
                            


                        }
                        // xóa những điều phối không được gửi lên
                        var dieuPhoi2Remove = lstMapping.Where(e => !newMappingId.Contains(e.MappingId)).ToList();

                        _context.OrderMapping.RemoveRange(dieuPhoi2Remove);
                        _context.SaveChanges();
                        ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Thêm thông tin thành công" };
                        ret.isSuccess = true;
                    }
                    else
                    {
                        ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Dữ liệu không đúng" };
                        ret.isSuccess = false;
                    }
                }
            }
            return ret;
        }
    }
}