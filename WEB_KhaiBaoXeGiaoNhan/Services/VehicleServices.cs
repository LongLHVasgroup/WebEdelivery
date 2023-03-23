using Dapper;
using Microsoft.AspNetCore.Builder;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WEB_KhaiBaoXeGiaoNhan.Datalayers;
using WEB_KhaiBaoXeGiaoNhan.Models;
using WEB_KhaiBaoXeGiaoNhan.WebModels;
using WEB_KhaiBaoXeGiaoNhan.WebModelsPMC;

namespace WEB_KhaiBaoXeGiaoNhan.Services
{
    public class VehicleServices : BaseService<VehicleServices>
    {
        /// <summary>
        /// lấy danh sách xe
        /// </summary>
        /// <param name="username"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<ResponseVehicle> GetAll(string username, string type)
        {
            List<ResponseVehicle> ret = null;
            try
            {
                using (var web_context = new Web_BookingTransContext())
                {
                    var user = UserModelDAO.GetInstance().GetList()
                                                        .Where(u => u.Username == username).FirstOrDefault();
                    if (user != null)
                    {
                        List<ResponseVehicle> vehicles = null;

                        if (user.UserType == Constants.UserConstants.UserTypeC)
                        {

                            /*
                            var customer = CustomerModelDAO.GetInstance().GetList()
                                                            .Where(c => c.CustomerId == user.Memberof)
                                                            .FirstOrDefault();

                            if (customer != null)
                            {
                                var vehicleOwner = VehicleOwnerModelDAO.GetInstance().GetList()
                                                                        .Where(vo => vo.CustomerCode == customer.CustomerCode)
                                                                        .FirstOrDefault();
                                if (vehicleOwner != null)
                                {
                                    vehicle = VehicleModelDAO.GetInstance().GetList()
                                                                  .Where(v => v.VehicleOwner == vehicleOwner.VehicleOwner)
                                                                  .ToList();

                                    // lọc theo loại xe
                                    switch (type)
                                    {
                                        case "romooc":
                                            vehicle = vehicle.Where(v => v.IsRoMooc == 1)
                                                                      .ToList();
                                            break;
                                        case "normal":
                                            vehicle = vehicle.Where(v => v.IsRoMooc != 1)
                                                                      .ToList();
                                            break;
                                        default:
                                            break;
                                    }

                                    if (vehicle.Count > 0)
                                    {
                                        ret = new List<WebModels.VehicleModel>();
                                        ret = vehicle;
                                    }
                                }
                            }*/
                        }
                        else
                        {
                            var provider = ProviderModelDAO.GetInstance().GetList()
                                                            .Where(c => c.ProviderId == user.Memberof)
                                                            .FirstOrDefault();
                            if (provider != null)
                            {
                                //xe bảng gốc
                                using (SqlConnection connection = new SqlConnection(Config.getInstance().connWeb))
                                {
                                    var queryPars = new DynamicParameters();


                                    //nếu là nhà cung cấp dịch vụ thì lấy mã S ngược lại thì mã V
                                    if (user.IsService == true)
                                    {
                                        queryPars.Add("@VehicleOwner", "S" + provider.ProviderCode);

                                        queryPars.Add("@VehicleOwnerService", "V" + provider.ProviderCode);
                                    }
                                    else
                                    {
                                        queryPars.Add("@VehicleOwner", "V" + provider.ProviderCode);
                                        queryPars.Add("@VehicleOwnerService", "V" + provider.ProviderCode);
                                    }
                              
                                    queryPars.Add("@VehicleOwnerCode", provider.ProviderCode);
                                    vehicles = connection.Query<ResponseVehicle>(@"select A.VehicleId, A.Type, A.VehicleNumber, A.IsRomooc, A.TrongLuongDangKiem, 
		                                            A.VehicleWeight, A.TyLeVuot, A.IsLock, A.IsLockEdit, A.IsContainer, A.IsDauKeo,
		                                            dr.DriverId , dr.DriverName , dr.DriverCardNo,
		                                            vm2.VehicleId as RomoocId , vm2.VehicleNumber as RomoocNumber from (
		                                            select * from VehicleModel vm 
		                                            WHERE (vm.VehicleOwner = @VehicleOwner or vm.VehicleOwner = @VehicleOwnerService) ) as A
		                                            LEFT join VehicleInfoMapping as vim on A.VehicleId = vim.VehicleId and vim.VehicleOwner = @VehicleOwnerCode
		                                            LEFT JOIN DriverRegister dr on vim.DriverId = dr.DriverId 
		                                            LEFT JOIN VehicleModel vm2 on vim.RomoocId = vm2.VehicleId ", queryPars).ToList();


                                    var vehiclesMap = connection.Query<ResponseVehicle>(@"select A.VehicleId, A.Type, A.VehicleNumber, A.IsRomooc, A.TrongLuongDangKiem, 
		                                            A.VehicleWeight, A.TyLeVuot, A.IsLock, A.IsLockEdit, A.IsContainer, A.IsDauKeo,
		                                            dr.DriverId , dr.DriverName , dr.DriverCardNo,
		                                            vm2.VehicleId as RomoocId , vm2.VehicleNumber as RomoocNumber from (
		                                            select vm.* from VehicleModel vm , Vehicle_VehicleOwner_Mapping vvom 
		                                            WHERE (vvom.VehicleOwner = @VehicleOwner or vvom.VehicleOwner = @VehicleOwnerService  )
		                                            and vvom.VehicleId = vm.VehicleId) as A
		                                            LEFT join VehicleInfoMapping as vim on A.VehicleId = vim.VehicleId and vim.VehicleOwner = @VehicleOwnerCode
		                                            LEFT JOIN DriverRegister dr on vim.DriverId = dr.DriverId 
		                                            LEFT JOIN VehicleModel vm2 on vim.RomoocId = vm2.VehicleId ", queryPars).ToList();

                                    if (vehiclesMap.Count > 0)
                                    {
                                        vehicles.AddRange(vehiclesMap);

                                        vehicles.Distinct();
                                    }

                                }




                                /*
                                vehicle = VehicleModelDAO.GetInstance().GetList()
                                                            .Where(v => v.VehicleOwner != null && v.VehicleOwner.Contains(provider.ProviderCode))
                                                            .OrderBy(v => v.VehicleNumber)
                                                            .Distinct()
                                                            .ToList();

                                #region Lấy thêm xe ở bảng Map

                                var vehicleOwner = VehicleOwnerModelDAO.GetInstance().GetList()
                                                                                                .Where(vo => vo.ProviderCode == provider.ProviderCode)
                                                                                                .FirstOrDefault();
                                if (vehicleOwner != null)
                                {
                                    //xe ở bản map
                                    var sub = web_context.VehicleVehicleOwnerMapping.Where(s => s.VehicleOwner == vehicleOwner.VehicleOwner).Select(s => s.VehicleId).ToList();
                                    var subVehicle = VehicleModelDAO.GetInstance().GetList()
                                                                  .Where(v => sub.Contains(v.VehicleId))
                                                                  .OrderBy(v => v.VehicleNumber)
                                                                  .ToList();
                                    if (subVehicle.Count > 0)
                                    {
                                        vehicle.AddRange(subVehicle);
                                    }
                                }
                                */

                                // lấy theo đúng loại xe
                                
                                switch (type)
                                {
                                    case "romooc":
                                        vehicles = vehicles.Where(v => v.IsRoMooc == 1)
                                                                  .ToList();
                                        break;
                                    case "normal":
                                        vehicles = vehicles.Where(v => v.IsRoMooc != 1)
                                                                  .ToList();
                                        break;
                                    default:

                                        break;
                                }


                                if (vehicles.Count > 0)
                                {
                                    ret = new List<ResponseVehicle>();
                                    ret = vehicles;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ret;
        }

        /// <summary>
        /// tạo xe mới
        /// </summary>
        /// <param name="item"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int CreateNew(CreateVehicle item, string username)
        {
            int ret = 0;
            WebModels.VehicleModel model = new WebModels.VehicleModel();

            // map data
            model.VehicleNumber = item.VehicleNumber;
            model.IsDauKeo = item.IsDauKeo;
            model.IsRoMooc = item.IsRoMooc;
            model.VehicleWeight = item.VehicleWeight;
            model.TrongLuongDangKiem = item.TrongLuongDangKiem;


            try
            {
                using (var _context = new Web_BookingTransContext())
                {
                    var user = UserModelDAO.GetInstance().GetList()
                                                                   .Where(u => u.Username == username).FirstOrDefault();
                    if (user != null)
                    {
                        if (user.UserType == Constants.UserConstants.UserTypeC)
                        {
                            var customer = CustomerModelDAO.GetInstance().GetList()
                                                           .Where(c => c.CustomerId == user.Memberof)
                                                           .FirstOrDefault();
                            if (customer != null)
                            {
                                var vehicleOwner = VehicleOwnerModelDAO.GetInstance().GetList()
                                                                       .Where(vo => vo.CustomerCode == customer.CustomerCode)
                                                                       .FirstOrDefault();
                                if (vehicleOwner != null)
                                {
                                    var info = VehicleModelDAO.GetInstance().GetList()
                                        .Where(v => v.VehicleNumber.Replace("-", "").Replace(".", "").Replace(" ", "") ==
                                                    item.VehicleNumber.Replace("-", "").Replace(".", "").Replace(" ", ""))
                                        .Where(v => v.VehicleOwner == vehicleOwner.VehicleOwner)
                                        .ToList();
                                    var driver = DriverRegisterDAO.GetInstance().GetList().Where(d => d.DriverId == item.DriverId).FirstOrDefault();
                                    if (info.Count == 0)
                                    {
                                        model.VehicleId = Guid.NewGuid();
                                        model.Type = 2;
                                        model.VehicleOwner = vehicleOwner.VehicleOwner;
                                        /*
                                        if (driver != null)
                                        {
                                            model.DriverId = driver.DriverId;
                                            model.DriverName = driver.DriverName;
                                            model.DriverCardNo = driver.DriverCardNo;
                                        }*/
                                        // Check loại xe
                                        if (model.IsRoMooc == null)
                                            model.IsRoMooc = 0;
                                        model.VehicleNumber = item.VehicleNumber.ToUpper();
                                        model.TyLeVuot = 10;
                                        //item.IsRoMooc = 0;
                                        model.IsLock = true;
                                        model.IsLockEdit = true;
                                        model.CreatedUserId = user.Userid;
                                        model.CreatedTime = DateTime.Now;
                                        model.LastEditUserId = user.Userid;
                                        model.LastEditTime = DateTime.Now;
                                        ret = VehicleModelDAO.GetInstance().InsertOne(model);
                                    }
                                    else
                                    {
                                    }
                                }
                            }
                        }
                        else
                        {
                            var provider = ProviderModelDAO.GetInstance().GetList()
                                                            .Where(c => c.ProviderId == user.Memberof)
                                                            .FirstOrDefault();
                            if (provider != null)
                            {
                                //tìm xem thông tin xe đã tồn tại chưa
                                var info = VehicleModelDAO.GetInstance().GetList()
                                        .Where(v => v.VehicleNumber.Replace("-", "").Replace(".", "").Replace(" ", "").ToUpper().Equals(
                                                    item.VehicleNumber.Replace("-", "").Replace(".", "").Replace(" ", "").ToUpper()))
                                        .Where(v => v.VehicleOwner != null)
                                        //&& v.VehicleOwner.Contains(provider.ProviderCode) == false)//xe tồn tại, nhưng không phải provider là chính
                                        .ToList();
                                //nếu xe chưa tồn tại
                                if (info.Count == 0)
                                {
                                    //tạo thông tin mới
                                    model.VehicleId = Guid.NewGuid();
                                    model.Type = 2;
                                    //nếu có bind với driver
                                    /*var driver = DriverRegisterDAO.GetInstance().GetList().Where(d => d.DriverId == item.DriverId).FirstOrDefault();
                                    if (driver != null)
                                    {
                                        model.DriverId = driver.DriverId;
                                        model.DriverName = driver.DriverName;
                                        model.DriverCardNo = driver.DriverCardNo;
                                    }*/
                                    //tìm mã owner
                                    var vehicleOwner = VehicleOwnerModelDAO.GetInstance().GetList()
                                                                    .Where(vo => vo.ProviderCode == provider.ProviderCode)
                                                                    .FirstOrDefault();

                                    //nếu có owner
                                   
                                    if (vehicleOwner != null)
                                    {
                                        //nếu user là dịch vụ vận chuyển
                                        if (user.IsService == true)
                                        {
                                            var vehecleOwerWithS = VehicleOwnerModelDAO.GetInstance().GetList()
                                                                    .Where(vo => vo.ProviderCode == provider.ProviderCode && vo.VehicleOwner.StartsWith("S")).FirstOrDefault();

                                            model.VehicleOwner = vehecleOwerWithS.VehicleOwner;
                                        }
                                        else
                                        {
                                            var vehecleOwerWithV = VehicleOwnerModelDAO.GetInstance().GetList()
                                                                   .Where(vo => vo.ProviderCode == provider.ProviderCode && vo.VehicleOwner.StartsWith("V")).FirstOrDefault();
                                            model.VehicleOwner = vehecleOwerWithV.VehicleOwner;
                                        }
                                       
                                    }
                                    //nếu không có thì tự thêm V
                                    else
                                    {
                                        //nếu user là dịch vụ vận chuyển
                                        if (user.IsService == true)
                                        {

                                            model.VehicleOwner = "S" + provider.ProviderCode;
                                        }
                                        else
                                        {
                                            model.VehicleOwner = "V" + provider.ProviderCode;
                                        }
                                          
                                    }
                                    model.VehicleNumber = item.VehicleNumber.ToUpper();
                                    model.TyLeVuot = 10;
                                    // Check loại xe
                                    if (model.IsRoMooc == null)
                                        model.IsRoMooc = 0;
                                    //item.IsRoMooc = 0;
                                    model.IsLock = true;
                                    model.IsLockEdit = true;
                                    model.CreatedUserId = user.Userid;
                                    model.CreatedTime = DateTime.Now;
                                    model.LastEditUserId = user.Userid;
                                    model.LastEditTime = DateTime.Now;
                                    //thêm xe mới vào
                                    ret = VehicleModelDAO.GetInstance().InsertOne(model);
                                    // thêm mới vào bảng Info Mapping
                                    try
                                    {
                                        VehicleInfoMapping vehicleInfoMapping = new VehicleInfoMapping();
                                        vehicleInfoMapping.VehicleId = model.VehicleId;
                                        vehicleInfoMapping.VehicleOwner = provider.ProviderCode;
                                        vehicleInfoMapping.RomoocId = item.RomoocId;
                                        vehicleInfoMapping.DriverId = item.DriverId;

                                        VehicleInfoMappingDAO.GetInstance().InsertOne(vehicleInfoMapping);
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                                else
                                {
                                    // có rồi nếu cùng ower thì không cần thêm
                                    foreach (var vehicleItem in info)
                                    {
                                        if (vehicleItem.VehicleOwner.Equals("V" + provider.ProviderCode) || vehicleItem.VehicleOwner.Equals("S" + provider.ProviderCode))
                                        {
                                            return -1;
                                        }

                                    }
                                    // xe mới đối với nhà cung cấp thì thêm vào mapping

                                    var vehicle = info.FirstOrDefault();
                                    var newitem = new WebModels.VehicleVehicleOwnerMapping
                                    {
                                        VehicleId = vehicle.VehicleId,
                                        VehicleOwner = user.IsService.Value? "S" + provider.ProviderCode: "V" + provider.ProviderCode
                                    };
                                    _context.VehicleVehicleOwnerMapping.Add(newitem);
                                    ret = _context.SaveChanges();

                                    // Thêm bên bảng info
                                    try
                                    {
                                        VehicleInfoMapping vehicleInfoMapping = new VehicleInfoMapping();
                                        vehicleInfoMapping.VehicleId = newitem.VehicleId;
                                        vehicleInfoMapping.VehicleOwner = provider.ProviderCode;
                                        vehicleInfoMapping.RomoocId = item.RomoocId;
                                        vehicleInfoMapping.DriverId = item.DriverId;

                                        VehicleInfoMappingDAO.GetInstance().InsertOne(vehicleInfoMapping);
                                    }
                                    catch (Exception)
                                    {

                                    }
                                    /*
                                    //thêm dưới pmc
                                    using (var _4000Context = new Vas_4000Context())
                                    {
                                        var new_4000item = new WebModelsPMC.VehicleVehicleOwnerMapping
                                        {
                                            VehicleId = vehicle.VehicleId,
                                            VehicleOwner = "V" + provider.ProviderCode
                                        };
                                        _4000Context.VehicleVehicleOwnerMapping.Add(new_4000item);
                                        _4000Context.SaveChanges();
                                    }*/
                                    //ret = _context.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ret;
        }

        /// <summary>
        /// xóa xe đã tạo
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(string userName, Guid id)
        {
            int ret = 0;
            string vehicleOwner = "";
            var user = UserModelDAO.GetInstance().GetList()
                .Where(u => u.Username == userName).FirstOrDefault();
            if (user == null)
            {
                return ret;
            }
            var vehicle = VehicleModelDAO.GetInstance().GetList()
                                        .Where(v => v.VehicleId == id)
                                        .FirstOrDefault();
            if (vehicle == null)
            {
                return ret;
            }

            using (SqlConnection connection = new SqlConnection(Config.getInstance().connWeb))
            {
                var queryPars = new DynamicParameters();
                queryPars.Add("@VehicleId", id);
                var vehicleOwnerMapping = connection.Query<WebModels.VehicleVehicleOwnerMapping>(@"select * from dbo.Vehicle_VehicleOwner_Mapping where VehicleId = @VehicleId", queryPars).FirstOrDefault();
                if (vehicleOwnerMapping == null)
                {
                    // Xóa luôn trên bảng chính
                    ret = VehicleModelDAO.GetInstance().DeleteOne(vehicle);
                    if(ret != 1)
                    {
                        return ret;
                    }

                    queryPars.Add("@VehicleOwner", vehicleOwner);
                    var vehicleInfoMapping = connection.Query<WebModels.VehicleInfoMapping>(@"select * from dbo.VehicleInfoMapping WITH(NOLOCK)
                                                                where VehicleId =@VehicleId and VehicleOwner = @VehicleOwner", queryPars)
                        .FirstOrDefault();

                    if (vehicleInfoMapping != null)
                    {
                        ret = VehicleInfoMappingDAO.GetInstance().DeleteOne(vehicleInfoMapping);
                    }

                    return ret;
                }
            }
            // nếu xe của nhiều nhà thì cập nhật lại owner mới

            if (user.UserType == Constants.UserConstants.UserTypeP)
            {
                using (SqlConnection connection = new SqlConnection(Config.getInstance().connWeb))
                {

                    // lấy mã nhà cung cấp
                    var queryPars = new DynamicParameters();
                    queryPars.Add("@ProviderId", user.Memberof);
                    queryPars.Add("@VehicleId", id);

                    var provider = connection.Query<WebModels.ProviderModel>(@"select * from dbo.ProviderModel where ProviderId = @ProviderId", queryPars).FirstOrDefault();
                    if (provider == null)
                    {
                        return ret;
                    }
                    vehicleOwner = provider.ProviderCode;

                    string vehicleOwnerCode = "V" + provider.ProviderCode;
                    queryPars.Add("@VehicleOwner", vehicleOwnerCode);
                    if (vehicle.VehicleOwner == vehicleOwnerCode)
                    {
                        // Cập nhật owner bản chính và xóa bên bảng mapping
                        var ownerMapping = connection.Query<WebModels.VehicleVehicleOwnerMapping>(@"select * from dbo.Vehicle_VehicleOwner_Mapping WITH(NOLOCK) where VehicleId = @VehicleId", queryPars).FirstOrDefault();
                        vehicle.VehicleOwner = ownerMapping.VehicleOwner;
                        vehicle.LastEditUserId = user.Userid;
                        vehicle.LastEditTime = DateTime.Now;
                        // xóa bên bảng mapping
                        ret = VehicleModelDAO.GetInstance().UpdateOne(vehicle);
                        if (ret == 1)
                        {
                            VehicleVehicleOwnerMappingDAO.GetInstance().DeleteOne(ownerMapping);
                        }
                    }
                    else
                    {
                        // chỉ  xóa bên bảng mapping
                        var ownerMapping = connection.Query<WebModels.VehicleVehicleOwnerMapping>(@"select * from dbo.Vehicle_VehicleOwner_Mapping WITH(NOLOCK) where VehicleId = @VehicleId and VehicleOwner = @VehicleOwner", queryPars).FirstOrDefault();
                        ret = VehicleVehicleOwnerMappingDAO.GetInstance().DeleteOne(ownerMapping);
                    }
                }
            }

            // Xóa trong bảng reference
            if (ret == 1)
            {
                using (SqlConnection connection = new SqlConnection(Config.getInstance().connWeb))
                {
                    var queryPars = new DynamicParameters();
                    queryPars.Add("@VehicleOwner", vehicleOwner);
                    queryPars.Add("@VehicleId", id);
                    var vehicleInfoMapping = connection.Query<WebModels.VehicleInfoMapping>(@"select * from dbo.VehicleInfoMapping WITH(NOLOCK)
                                                                where VehicleId =@VehicleId and VehicleOwner = @VehicleOwner", queryPars)
                        .FirstOrDefault();

                    if(vehicleInfoMapping != null)
                    {
                        ret = VehicleInfoMappingDAO.GetInstance().DeleteOne(vehicleInfoMapping);
                    }
                }
                
            }

            return ret;
        }

        /// <summary>
        /// Cập nhật thông tin tài xế theo xe
        /// </summary>
        /// <param name="username"></param>
        /// <param name="vehicleId"></param>
        /// <param name="driverId"></param>
        /// <param name="romoocId"></param>
        /// <returns></returns>
        public ActionMessage Update(string username, Guid vehicleId, Guid driverId, Guid romoocId)
        {
            var ret = new ActionMessage();

            using (SqlConnection connection = new SqlConnection(Config.getInstance().connWeb))
            {
                var queryPars = new DynamicParameters();
                queryPars.Add("@UserName", username);
                var user = connection.Query<WebModels.UserModel>(@"select * from dbo.UserModel where username = @UserName", queryPars).FirstOrDefault();
                if (user == null)
                {
                    ret.isSuccess = false;
                    ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Thông tin người dùng không đúng!" };
                    return ret;
                }
                if (user.UserType == Constants.UserConstants.UserTypeP)
                {
                    // Get providercode
                    queryPars.Add("@ProviderId", user.Memberof);
                    var providerCode = connection.Query<string>(@"select ProviderCode from dbo.ProviderModel where ProviderId = @ProviderId", queryPars).FirstOrDefault();

                    // check đã có thông tin hay chưa
                    var updateVehicle = new DynamicParameters();
                    updateVehicle.Add("@VehicleId", vehicleId);
                    updateVehicle.Add("@VehicleOwner", providerCode);
                    updateVehicle.Add("@RomoocId", romoocId);
                    updateVehicle.Add("@DriverId", driverId);

                    var maper = connection.Query<VehicleInfoMapping>(@"select * from dbo.VehicleInfoMapping 
                                                            where VehicleId = @VehicleId and VehicleOwner = @VehicleOwner"
                                            , updateVehicle).FirstOrDefault();

                    try
                    {
                        if (maper == null)
                        {
                            // Insert
                            connection.Execute(@" INSERT INTO dbo.VehicleInfoMapping
                                                    ( VehicleId,
                                                    VehicleOwner ,
                                                    RomoocId ,
                                                    DriverId
                                                    )
                                                    values(
                                                            @VehicleId,
                                                            @VehicleOwner ,
                                                            @RomoocId,
                                                            @DriverId
                                                            )", updateVehicle);
                        }
                        else
                        {
                            connection.Execute(@"update VehicleInfoMapping set
                                                    RomoocId = @RomoocId ,
                                                    DriverId = @DriverId 
                                             where VehicleId = @VehicleId and VehicleOwner = @VehicleOwner", updateVehicle);
                        }
                    }
                    catch (Exception ex)
                    {
                        ret.isSuccess = false;
                        ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Cập nhật không thành công!" };
                        return ret;
                    }
                }
                ret.isSuccess = true;
                ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Cập nhật thành công!" };
            }


            /*
            var vehicle = VehicleModelDAO.GetInstance().GetList().Where(v => v.VehicleId == vehicleId).FirstOrDefault();
            if (vehicle != null)
            {
                var driver = DriverRegisterDAO.GetInstance().GetList().Where(d => d.DriverId == driverId).FirstOrDefault();
                if (driver != null)
                {
                    vehicle.DriverId = driver.DriverId;
                    vehicle.DriverName = driver.DriverName;
                    vehicle.DriverCardNo = driver.DriverCardNo;

                    if (VehicleModelDAO.GetInstance().UpdateOne(vehicle) > 0)
                    {
                        ret.isSuccess = true;
                        ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Cập nhật thành công!" };
                    }
                    else
                    {
                        ret.isSuccess = false;
                        ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Cập nhật thất bại!" };
                    }
                }
                else
                {
                    ret.isSuccess = false;
                    ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Không tìm thấy thông tin tài xế!" };
                }
            }
            else
            {
                ret.isSuccess = false;
                ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Không tìm thấy thông tin xe!" };
            }
            */
            return ret;
        }
    }
}