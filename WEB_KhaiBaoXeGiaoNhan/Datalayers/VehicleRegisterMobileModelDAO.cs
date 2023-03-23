using AdminPortal.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Datalayers
{
    public class VehicleRegisterMobileModelDAO : DataProvider<VehicleRegisterMobileModelDAO>
    {
        public List<VehicleRegisterMobileModel> GetList()
        {
            using (Web_BookingTransContext context = new Web_BookingTransContext())
            {
                return context.VehicleRegisterMobileModel.ToList();
            }
        }

        public int InsertOne(VehicleRegisterMobileModel item)
        {
            int result = 0;
            using (Web_BookingTransContext context = new Web_BookingTransContext())
            {
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.VehicleRegisterMobileModel.Add(item);
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

        public int UpdateOne(VehicleRegisterMobileModel item)
        {
            int result = 0;
            using (Web_BookingTransContext context = new Web_BookingTransContext())
            {
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.VehicleRegisterMobileModel.Update(item);
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

        public int DeleteOne(VehicleRegisterMobileModel item)
        {
            int result = 0;
            using (Web_BookingTransContext context = new Web_BookingTransContext())
            {
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.VehicleRegisterMobileModel.Remove(item);
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