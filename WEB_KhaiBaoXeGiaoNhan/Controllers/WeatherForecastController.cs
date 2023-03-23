using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace WEB_KhaiBaoXeGiaoNhan.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : BaseController
    {
        [AllowAnonymous]
        [HttpGet("Identity")]
        public SingleResponeMessage<System.Security.Principal.IIdentity> getIdentity()
        {
            var ret = new SingleResponeMessage<System.Security.Principal.IIdentity>();
            ret.item = GetIdentity();
            return ret;
        }

        [AllowAnonymous]
        [HttpGet("Claim")]
        public ListResponeMessage<Claim> getClaim()
        {
            var ret = new ListResponeMessage<Claim>();
            ret.data = GetClaim();
            return ret;
        }

        [AllowAnonymous]
        [HttpGet("ClaimsIdentity")]
        public ListResponeMessage<ClaimsIdentity> getClaimsIdentity()
        {
            var ret = new ListResponeMessage<ClaimsIdentity>();
            ret.data = GetClaimsIdentity();
            return ret;
        }
    }
}