using AdminPortal.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Datalayers
{
    public class VehicleInfoMappingDAO : DataProvider<VehicleInfoMappingDAO>
    {
        public List<VehicleInfoMapping> GetList()
        {
            using (Web_BookingTransContext context = new Web_BookingTransContext())
            {
                return context.VehicleInfoMapping.ToList();
            }
        }

        public int InsertOne(VehicleInfoMapping item)
        {
            int result = 0;
            using (Web_BookingTransContext context = new Web_BookingTransContext())
            {
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.VehicleInfoMapping.Add(item);
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

        public int UpdateOne(VehicleInfoMapping item)
        {
            int result = 0;
            using (Web_BookingTransContext context = new Web_BookingTransContext())
            {
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.VehicleInfoMapping.Update(item);
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
        public int DeleteOne(VehicleInfoMapping item)
        {
            int result = 0;
            using (Web_BookingTransContext context = new Web_BookingTransContext())
            {
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.VehicleInfoMapping.Remove(item);
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