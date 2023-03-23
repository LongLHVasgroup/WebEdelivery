using AdminPortal.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Datalayers
{
    public class VehicleRegisterPODetailModelDAO : DataProvider<VehicleRegisterPODetailModelDAO>
    {
        public List<VehicleRegisterPodetailModel> GetList()
        {
            using (Web_BookingTransContext context = new Web_BookingTransContext())
            {
                return context.VehicleRegisterPodetailModel.ToList();
            }
        }

        public int InsertOne(VehicleRegisterPodetailModel item)
        {
            int result = 0;
            using (Web_BookingTransContext context = new Web_BookingTransContext())
            {
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.VehicleRegisterPodetailModel.Add(item);
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

        public int UpdateOne(VehicleRegisterPodetailModel item)
        {
            int result = 0;
            using (Web_BookingTransContext context = new Web_BookingTransContext())
            {
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.VehicleRegisterPodetailModel.Update(item);
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

        public int DeleteOne(VehicleRegisterPodetailModel item)
        {
            int result = 0;
            using (Web_BookingTransContext context = new Web_BookingTransContext())
            {
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.VehicleRegisterPodetailModel.Remove(item);
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