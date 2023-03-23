using AdminPortal.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Datalayers
{
    public class RegisterForShipDAO : DataProvider<RegisterForShipDAO>
    {
        public List<RegisterForShip> GetList()
        {
            using (Web_BookingTransContext context = new Web_BookingTransContext())
            {
                return context.RegisterForShip.ToList();
            }
        }

        public int InsertOne(RegisterForShip item)
        {
            int result = 0;
            using (Web_BookingTransContext context = new Web_BookingTransContext())
            {
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.RegisterForShip.Add(item);
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

        public int UpdateOne(RegisterForShip item)
        {
            int result = 0;
            using (Web_BookingTransContext context = new Web_BookingTransContext())
            {
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.RegisterForShip.Update(item);
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

        public int DeleteOne(RegisterForShip item)
        {
            int result = 0;
            using (Web_BookingTransContext context = new Web_BookingTransContext())
            {
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.RegisterForShip.Remove(item);
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
