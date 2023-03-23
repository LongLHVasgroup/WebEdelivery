using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace WEB_KhaiBaoXeGiaoNhan.Controllers
{
    [Authorize]
    public class BaseController : ControllerBase
    {
        protected static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get subject value of token
        /// </summary>
        /// <returns>Value subject of token</returns>
        protected string GetUserId()
        {
            //lấy subject từ token
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId;
        }

        /// <summary>
        /// Log Object
        /// </summary>
        /// <param name="obj"></param>
        public void DumpObject(object obj)
        {
            StringBuilder builder = new StringBuilder("");
            var dumbObj = ObjectDumper.Dump(obj, DumpStyle.CSharp);
            var type = obj.GetType().Name;
            builder.Append(string.Format("Preparing dumb {0}: ", type));
            builder.Append(dumbObj);
            _logger.Debug(builder.ToString());
        }

        protected System.Security.Principal.IIdentity GetIdentity()
        {
            return this.User.Identity;
        }

        protected List<Claim> GetClaim()
        {
            return this.User.Claims.ToList();
        }

        protected List<ClaimsIdentity> GetClaimsIdentity()
        {
            return this.User.Identities.ToList();
        }
    }
}