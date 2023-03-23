using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_KhaiBaoXeGiaoNhan.Models;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderPriceController : BaseController
    {
        /// <summary>
        /// Danh Sách po giá khác
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ListResponeMessage<PoResponseModel> GetList()
        {
            var ret = new ListResponeMessage<PoResponseModel>();
            try
            {
                var data = new List<PoResponseModel>();
                using (var _context = new Web_BookingTransContext())
                {
                    //đã có thông tin map, chưa bị deleted
                    var orderpricepo = _context.PogiaKhacMapping.Where(e => e.Deleted == false).Select(e => e.PoNumber).ToList();
                    var po = _context.PomasterModel.Where(e => orderpricepo.Contains(e.Ponumber)).ToList();
                    var poline = _context.PolineModel.Where(e => orderpricepo.Contains(e.Ponumber)).ToList();
                    for (int i = 0; i < po.Count; i++)
                    {
                        var item = new PoResponseModel
                        {
                            Pomasters = po[i],
                            Polines = poline.Where(e => e.Ponumber == po[i].Ponumber).ToList()
                        };
                        data.Add(item);
                    }
                }
                ret.data = data;
                ret.isSuccess = true;
                ret.totalRecords = data.Count;
                ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Danh sách po giá khác" };
            }
            catch (Exception)
            {
                throw;
            }
            return ret;
        }

        /// <summary>
        /// Map giá khác cho po
        /// </summary>
        [HttpPost("create")]
        public ActionMessage CreateNew(string ponumber)
        {
            var ret = new ActionMessage();
            try
            {
                using (var _context = new Web_BookingTransContext())
                {
                    var check = _context.PogiaKhacMapping.Where(e => e.PoNumber == ponumber && e.Deleted == false).FirstOrDefault();
                    if (check == null)
                    {
                        var user = _context.UserModel.Where(e => e.Username == GetUserId()).FirstOrDefault();
                        var item = new PogiaKhacMapping
                        {
                            Id = Guid.NewGuid(),
                            PoNumber = ponumber,
                            CreatedTime = DateTime.Now,
                            Creator = user.Userid,
                            Deleted = false,
                            IsGiaKhac = true,
                        };
                        _context.PogiaKhacMapping.Add(item);
                        if (_context.SaveChanges() == 1)
                        {
                            ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Thêm thành công" };
                            ret.isSuccess = true;
                        }
                        else
                        {
                            ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Thêm không thành công" };
                            ret.isSuccess = false;
                        }
                    }
                    else
                    {
                        ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Đã tồn tại thông tin giá khác" };
                        ret.isSuccess = false;
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
        /// Map giá khác cho po
        /// </summary>
        [HttpPut("delete")]
        public ActionMessage Delete(string ponumber)
        {
            var ret = new ActionMessage();
            try
            {
                using (var _context = new Web_BookingTransContext())
                {
                    var user = _context.UserModel.Where(e => e.Username == GetUserId()).FirstOrDefault();
                    var item = _context.PogiaKhacMapping.Where(e => e.PoNumber == ponumber && e.Deleted != true).FirstOrDefault();

                    if (item != null)
                    {
                        item.Modifier = user.Userid;
                        item.Deleted = true;
                        _context.PogiaKhacMapping.Update(item);
                        if (_context.SaveChanges() == 1)
                        {
                            ret.err = new ErorrMssage { msgCode = "2xx", msgString = "Cập nhật thành công" };
                            ret.isSuccess = true;
                        }
                        else
                        {
                            ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Cập nhật không thành công" };
                            ret.isSuccess = false;
                        }
                    }
                    else
                    {
                        {
                            ret.err = new ErorrMssage { msgCode = "4xx", msgString = "Không tìm thấy thông tin" };
                            ret.isSuccess = false;
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
    }
}