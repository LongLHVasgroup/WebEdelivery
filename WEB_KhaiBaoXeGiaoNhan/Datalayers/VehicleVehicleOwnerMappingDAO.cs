using AdminPortal.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Datalayers
{
    public class VehicleVehicleOwnerMappingDAO : DataProvider<VehicleVehicleOwnerMappingDAO>
    {

        public int DeleteOne(VehicleVehicleOwnerMapping item)
        {
            int result = 0;
            using (Web_BookingTransContext context = new Web_BookingTransContext())
            {
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.VehicleVehicleOwnerMapping.Remove(item);
                        result = context.SaveChanges();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        WriteLogErr(ex.Message);
                    }
                }
            }
            return result;
        }
    }
}