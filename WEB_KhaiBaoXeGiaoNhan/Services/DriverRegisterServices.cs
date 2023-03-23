using System;
using System.Collections.Generic;
using System.Linq;
using WEB_KhaiBaoXeGiaoNhan.Datalayers;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Services
{
    public class DriverRegisterServices : BaseService<DriverRegisterServices>
    {
        /// <summary>
        /// lấy danh sách tài xế theo công ty
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public List<DriverRegister> GetList(string username)
        {
            List<DriverRegister> ret = null;
            if (!string.IsNullOrEmpty(username))
            {
                ret = new List<DriverRegister>();
                var user = UserModelDAO.GetInstance().GetList()
                                .Where(u => u.Username == username).FirstOrDefault();
                if (user != null)
                {
                    var driver = DriverRegisterDAO.GetInstance().GetList()
                                .Where(d => d.OwnerId == user.Memberof && d.Active == true).ToList();
                    if (driver.Count > 0)
                    {
                        ret = driver;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// tìm tài xế theo tên
        /// </summary>
        /// <param name="username"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public List<DriverRegister> GetList(string username, string criteria)
        {
            List<DriverRegister> ret = null;
            if (!string.IsNullOrEmpty(username))
            {
                ret = new List<DriverRegister>();
                var user = UserModelDAO.GetInstance().GetList()
                                .Where(u => u.Username == username).FirstOrDefault();
                if (user != null)
                {
                    var driver = DriverRegisterDAO.GetInstance().GetList()
                                .Where(d => d.OwnerId == user.Memberof && d.Active == true)
                                .Where(d => d.DriverName.ToLower().Contains(criteria.ToLower()) ||
                                            d.DriverCardNo.Contains(criteria))
                                .ToList();
                    if (driver.Count > 0)
                    {
                        ret = driver;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// tạo mới tài xế
        /// </summary>
        /// <param name="item"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int CreateNew(DriverRegister item, string username)
        {
            var result = 0;
            var user = UserModelDAO.GetInstance().GetList()
                                   .Where(u => u.Username == username)
                                   .FirstOrDefault();
            if (user != null)
            {
                var driverInfo = DriverRegisterDAO.GetInstance().GetList()
                                                  .Where(d => d.OwnerId == user.Memberof
                                                  && d.DriverCardNo.Trim().Equals(item.DriverCardNo)
                                                  && d.Active == true)
                                                  .ToList();
                if (driverInfo.Count < 1)
                {
                    item.DriverId = Guid.NewGuid();
                    item.CreatedTime = DateTime.Now;
                    item.ModifiedTime = DateTime.Now;
                    item.Creator = user.Userid;
                    item.OwnerId = (Guid)user.Memberof;
                    item.Active = true;
                    result = DriverRegisterDAO.GetInstance().InsertOne(item);
                }
            }
            return result;
        }

        /// <summary>
        /// cập nhật thông tin tài xế
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Update(DriverRegister item)
        {
            var result = 0;
            var driverInfo = DriverRegisterDAO.GetInstance().GetList()
                                                  .Where(d => d.DriverId == item.DriverId && d.Active == true)
                                                  .FirstOrDefault();
            if (driverInfo != null)
            {
                var hadDriver = DriverRegisterDAO.GetInstance().GetList()
                                                  .Where(d => d.DriverCardNo == item.DriverCardNo
                                                  && d.Active == true
                                                  && d.DriverId != driverInfo.DriverId)
                                                  .FirstOrDefault();
                if (hadDriver == null)
                {
                    driverInfo.DriverName = item.DriverName;
                    driverInfo.DriverCardNo = item.DriverCardNo;
                    driverInfo.ModifiedTime = DateTime.Now;
                    result = DriverRegisterDAO.GetInstance().UpdateOne(driverInfo);
                }
            }
            return result;
        }

        /// <summary>
        /// xóa thông tin tài xế
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(Guid id)
        {
            var result = 0;
            var driverInfo = DriverRegisterDAO.GetInstance().GetList()
                                                 .Where(d => d.DriverId == id && d.Active == true)
                                                 .FirstOrDefault();
            if (driverInfo != null)
            {
                driverInfo.Active = false;
                driverInfo.ModifiedTime = DateTime.Now;
                result = DriverRegisterDAO.GetInstance().UpdateOne(driverInfo);
            }
            return result;
        }
    }
}