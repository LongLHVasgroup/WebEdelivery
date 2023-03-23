using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using WEB_KhaiBaoXeGiaoNhan.ADO;
using WEB_KhaiBaoXeGiaoNhan.Models;
using WEB_KhaiBaoXeGiaoNhan.Services;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : BaseController
    {
        [HttpGet]
        public SingleResponeMessage<OrderResponseModel> GetOrder(string companyCode)
        {
            var ret = new SingleResponeMessage<OrderResponseModel>();
            var username = GetUserId();
            var item = OrdersServices.GetInstance().GetOrder(username, companyCode);
            if (item != null)
            {
                ret.item = item;
                ret.isSuccess = true;
                ret.err.msgCode = "2xx";
                ret.err.msgString = "Success";
            }
            else
            {
                ret.item = item;
                ret.isSuccess = false;
                ret.err.msgCode = "4xx";
                ret.err.msgString = "Fail";
            }
            return ret;
        }

        [AllowAnonymous]
        [HttpGet("getporemain")]
        public ActionMessage check()
        {
            ActionMessage ret = new ActionMessage();
            //var result = GetDataFromFunction.GetInstance().GetSLDaNhapTuPONumber("4500010763", "", "");
            ret.err.msgString = GetUserId();
            ret.isSuccess = true;
            return ret;
        }

        [AllowAnonymous]
        [HttpGet("Find")]
        public ListResponeMessage<PoResponseModel> Find(string ponumber)
        {
            var ret = new ListResponeMessage<PoResponseModel>();
            var data = new List<PoResponseModel>();
            try
            {
                using (var _context = new Web_BookingTransContext())
                {
                    var po = _context.PomasterModel.Where(e => e.Ponumber.Contains(ponumber)).ToList();
                    var poline = _context.PolineModel.Where(e => e.Ponumber.Contains(ponumber)).ToList();
                    for (int i = 0; i < po.Count; i++)
                    {
                        var item = new PoResponseModel();
                        item.Pomasters = po[i];
                        item.Polines = poline.Where(e => e.Ponumber == po[i].Ponumber).ToList();
                        data.Add(item);
                    }
                }
                ret.isSuccess = true;
                ret.data = data;
                ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Thông tin PO" };
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                throw;
            }
            return ret;
        }
    }
}