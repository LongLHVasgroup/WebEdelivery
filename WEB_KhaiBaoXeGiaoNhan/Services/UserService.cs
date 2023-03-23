using CryptoHelper;
using Models.Common;
using System.Linq;
using WEB_KhaiBaoXeGiaoNhan.Datalayers;
using WEB_KhaiBaoXeGiaoNhan.Models;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Services
{
    public class UserService : BaseService<UserService>
    {
        public UserModel GetUser(string criteria)
        {
            return UserModelDAO.GetInstance().GetList().Where(u => u.Username == criteria).FirstOrDefault();
        }

        /// <summary>
        /// lấy thông tin theo username
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public UserResponseModel GetUserInfo(string criteria)
        {
            UserResponseModel ret = null;
            var UserItem = UserModelDAO.GetInstance().GetList().Where(u => u.Username == criteria).FirstOrDefault();
            if (UserItem != null)
            {
                if (UserItem.UserType.Equals("Customer"))
                {
                    var CompanyItem = CustomerModelDAO.GetInstance().GetList().Where(c => c.CustomerId == UserItem.Memberof).FirstOrDefault();
                    if (CompanyItem != null)
                    {
                        ret = new UserResponseModel();
                        ret.Address = CompanyItem.Address;
                        ret.FullName = UserItem.Fullname;
                        ret.Company = CompanyItem.CustomerName;
                        ret.Email = UserItem.Email;
                        ret.Phone = UserItem.Phone;
                        ret.Type = UserItem.UserType;
                        ret.Taxcode = UserItem.Taxcode;
                        ret.IsService = UserItem.IsService;
                    }
                }
                else
                {
                    var CompanyItem = ProviderModelDAO.GetInstance().GetList().Where(c => c.ProviderId == UserItem.Memberof).FirstOrDefault();
                    if (CompanyItem != null)
                    {
                        ret = new UserResponseModel();
                        ret.Address = CompanyItem.Address;
                        ret.FullName = UserItem.Fullname;
                        ret.Company = CompanyItem.ProviderName;
                        ret.Email = UserItem.Email;
                        ret.Phone = UserItem.Phone;
                        ret.Type = UserItem.UserType;
                        ret.Taxcode = UserItem.Taxcode;
                        ret.IsService = UserItem.IsService;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// đổi mật khẩu
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public ActionMessage PasswordChange(UpdateModel item)
        {
            var ret = new ActionMessage();
            var info = UserModelDAO.GetInstance().GetList()
                                       .Where(s => s.Username == item.Username)
                                       .FirstOrDefault();
            if (info != null)
            {
                if (info.Password.Equals(Encrypt.EncryptPassword(item.OldPassword, Config.getInstance().appSecret)))
                {
                    info.Password = Encrypt.EncryptPassword(item.NewPassword, Config.getInstance().appSecret);
                    var result = UserModelDAO.GetInstance().UpdateOne(info);
                    if (result > 0)
                    {
                        ret.isSuccess = true;
                        ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Thay đổi mật khẩu thành công" };
                    }
                    else
                    {
                        ret.isSuccess = false;
                        ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Thay đổi mật khẩu thất bại" };
                    }
                }
                else
                {
                    ret.isSuccess = false;
                    ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Mật khẩu cũ không đúng" };
                }
            }
            else
            {
                ret.isSuccess = false;
                ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Thông tin tài khoản không đúng" };
            }

            return ret;
        }

        /// <summary>
        /// cập nhật thông tin user
        /// </summary>
        /// <param name="item"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public ActionMessage UpdateInfo(UpdateInfoModel item, string username)
        {
            var ret = new ActionMessage();
            var info = UserModelDAO.GetInstance().GetList()
                                       .Where(s => s.Username == username)
                                       .FirstOrDefault();
            if (info != null)
            {
                info.Fullname = item.FullName;
                info.Email = item.Email;
                info.Phone = item.Phone;
                if (UserModelDAO.GetInstance().UpdateOne(info) > 0)
                {
                    ret.isSuccess = true;
                    ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Cập nhật thông tin thành công" };
                }
                else
                {
                    ret.isSuccess = false;
                    ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Cập nhật thông tin thất bại" };
                }
            }
            else
            {
                ret.isSuccess = false;
                ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Không tìm thấy thông tin tài khoản" };
            }
            return ret;
        }
    }
}