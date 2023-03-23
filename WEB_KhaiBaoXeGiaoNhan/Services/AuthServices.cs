using BearerHelper;
using CryptoHelper;
using Models;
using Models.Common;
using System;
using System.Linq;
using WEB_KhaiBaoXeGiaoNhan.Datalayers;
using WEB_KhaiBaoXeGiaoNhan.Models;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Services
{
    public class AuthServices : BaseService<AuthServices>
    {
        public AuthenticateModelRespone SignIn(AuthenticateModel user)
        {
            AuthenticateModelRespone ret = new AuthenticateModelRespone();
            if (user != null)
            {
                //kiểm tra thông tin đăng nhập
                var info = UserModelDAO.GetInstance().GetList()
                                        .Where(s => s.Username == user.userName && s.Active == true)
                                        .FirstOrDefault();
                if (info != null)
                {
                    //nếu mật khẩu đúng thì cấp token theo username
                    if (info.Password.Equals(Encrypt.EncryptPassword(user.password, Config.getInstance().appSecret)))
                    {
                        var token = new JwtTokenBuilder()
                                .AddSecurityKey(JwtSecurityKey.Create(Config.getInstance().getAppSecret()))
                                .AddSubject(info.Username)

                        #region optional

                                .AddIssuer("Fiver.Security.Bearer")
                                .AddAudience("Fiver.Security.Bearer")

                        #endregion optional

                                .AddClaim("sAdmin", "pass")
                                .AddExpiry(7)
                                .Build();
                        ret.token = token.Value;
                        ret.name = info.Fullname;
                        ret.role = info.Rolecode;
                        ret.userID = info.Userid;
                        ret.type = info.UserType;
                        ret.username = info.Username;
                        ret.isService = (bool)info.IsService;
                        ret.isSuccess = true;
                        ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Đăng nhập thành công" };
                    }
                    else
                    {
                        ret.isSuccess = false;
                        ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Sai mật khẩu" };
                    }
                }
                else
                {
                    ret.isSuccess = false;
                    ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Thông tin đăng nhập không tồn tại" };
                }
            }
            return ret;
        }

        public ActionMessage SignUp(RegisterModel model)
        {
            var ret = new ActionMessage();
            //kiểm tra xem tài khoản đó có tồn tại hay chưa
            var info = UserModelDAO.GetInstance().GetList()
                                       .Where(s => s.Username == model.Username)
                                       .FirstOrDefault();
            //nếu chưa thì tiếp tục
            if (info == null)
            {
                //xếp vào nhóm khách hoặc nhà cung cấp
                var groupID = (
                model.IsCustomer ?
                CustomerModelDAO
                .GetInstance()
                .GetList()
                .Where(c => c.CustomerCode == model.Owner)
                .Select(c => c.CustomerId)
                .FirstOrDefault()
                :
                ProviderModelDAO
                .GetInstance()
                .GetList()
                .Where(c => c.ProviderCode == model.Owner)
                .Select(c => c.ProviderId)
                .FirstOrDefault()
                );
                //nếu không có thì trả về lỗi
                if (groupID == Guid.Empty)
                {
                    ret.isSuccess = false;
                    ret.err.msgString = "Không tìm thấy nhà cung cấp hoặc khách hàng!";
                    ret.err.msgCode = "4xx";
                }
                else
                {
                    //nếu có nhà cung cấp hoặc khách hàng
                    var item = new UserModel
                    {
                        Userid = Guid.NewGuid(),
                        Username = model.Username,
                        Password = Encrypt.EncryptPassword(model.Password, Config.getInstance().appSecret),
                        Fullname = model.Fullname,
                        Email = model.Email,
                        Phone = model.Phone,
                        Memberof = groupID,
                        Active = true,
                        UserType = model.IsCustomer ? Constants.UserConstants.UserTypeC : Constants.UserConstants.UserTypeP,
                        IsService = false,
                        Rolecode = 1,
                        Taxcode = "0000000000",
                        Token = null
                    };
                    if (UserModelDAO.GetInstance().InsertOne(item) > 0)
                    {
                        ret.isSuccess = true;
                        ret.err.msgString = "Tạo tài khoản thành công!";
                        ret.err.msgCode = "2xx";
                    }
                }
            }
            else
            {
                ret.isSuccess = false;
                ret.err.msgString = "Tài khoản đã tồn tại!";
                ret.err.msgCode = "4xx";
            }
            return ret;
        }
    }
}