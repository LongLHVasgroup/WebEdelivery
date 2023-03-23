using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Common;
using WEB_KhaiBaoXeGiaoNhan.Models;
using WEB_KhaiBaoXeGiaoNhan.Services;

namespace WEB_KhaiBaoXeGiaoNhan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        [AllowAnonymous]
        [HttpPost("signin")]
        public AuthenticateModelRespone Signin([FromBody] AuthenticateModel auth)
        {
            var ret = new AuthenticateModelRespone();
            if (auth != null)
            {
                ret = AuthServices.GetInstance().SignIn(auth);
            }
            return ret;
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public ActionMessage Signup([FromBody] RegisterModel user)
        {
            var ret = new ActionMessage();
            if (user != null)
            {
                ret = AuthServices.GetInstance().SignUp(user);
            }
            return ret;
        }

        [HttpGet("check")]
        public ActionMessage check()
        {
            ActionMessage ret = new ActionMessage();
            ret.err.msgString = GetUserId();
            ret.isSuccess = true;
            return ret;
        }
    }
}