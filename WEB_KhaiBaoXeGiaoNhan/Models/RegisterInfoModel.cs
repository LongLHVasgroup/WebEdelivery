using System;
using System.Collections.Generic;

namespace WEB_KhaiBaoXeGiaoNhan.Models
{
    public class RegisterInfoModel
    {
        private string orderNumber;
        private DateTime ngayGiaoNhan;
        private List<RegisterDetailModel> listVehicle;
        private int? cungDuongCode;
        public string OrderNumber { get => orderNumber; set => orderNumber = value; }
        public DateTime NgayGiaoNhan { get => ngayGiaoNhan; set => ngayGiaoNhan = value; }
        public List<RegisterDetailModel> ListVehicle { get => listVehicle; set => listVehicle = value; }
        public int? CungDuongCode { get => cungDuongCode; set => cungDuongCode = value; }
    }

    public class RegisterDetailModel
    {
        private string vehicleNumber;
        private Guid driverID;
        private List<string> productCode;
        private decimal trongLuongDuKien;
        private string assets;
        private string note;
        private int bonusHour;
        private string romooc;
        private int soLuot;
        public string Assets { get => assets; set => assets = value; }
        public string VehicleNumber { get => vehicleNumber; set => vehicleNumber = value; }
        public List<string> ListProduct { get => productCode; set => productCode = value; }
        public decimal TrongLuongDuKien { get => trongLuongDuKien; set => trongLuongDuKien = value; }
        public string Note { get => note; set => note = value; }
        public Guid DriverID { get => driverID; set => driverID = value; }
        public int BonusHour { get => bonusHour; set => bonusHour = value; }
        public string Romooc { get => romooc; set => romooc = value; }
        public int SoLuot { get => soLuot; set => soLuot = value; }
    }

    public class Product
    {
        private string productCode;
        private string productName;

        public string ProductCode { get => productCode; set => productCode = value; }
        public string ProductName { get => productName; set => productName = value; }
    }
}