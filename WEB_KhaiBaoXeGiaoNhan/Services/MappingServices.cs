using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_KhaiBaoXeGiaoNhan.Models;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Services
{
    public class MappingServices : BaseService<MappingServices>
    {
        public List<OrderMapping> GetServices(string PoNumber)
        {
            var ret = new List<OrderMapping>();
            using (var _context = new Web_BookingTransContext())
            {
                var lst = _context.OrderMapping.Where(or => or.OrderNumber == PoNumber).ToList();
                if (lst != null)
                {
                    ret = lst;
                }
            }
            return ret;
        }

        public int ChangeIsDone(Guid id, string username)
        {
            int result = 0;
            using (var _context = new Web_BookingTransContext())
            {
                var dieuphoi = _context.OrderMapping.Where(or => or.MappingId == id).FirstOrDefault();
                if (dieuphoi == null)
                {
                    result = 0;
                    return result;
                }
                // cập nhật lại trạng thái mới
                
                if(dieuphoi.IsDone == null || dieuphoi.IsDone == false)
                {
                    dieuphoi.IsDone = true;
                }
                else
                {
                    dieuphoi.IsDone = false;
                }

                try
                {
                    _context.OrderMapping.Update(dieuphoi);
                    result = _context.SaveChanges();
                }
                catch (Exception ex)
                {
                }
            }

            return result;
        }
    }
}