using System;

namespace WEB_KhaiBaoXeGiaoNhan.Responses
{
    public class VehicleRegisForShipResponse
    {
        public int Id { get; set; }
        public string PoNumber { get; set; }
        public string DvvcCode { get; set; }
        public string Dvvc { get; set; }
        public string VehicleNumber { get; set; }
        public Guid DriverId1 { get; set; }
        public string DriverName1 { get; set; }
        public string DriverIdCard1 { get; set; }
        public Guid DriverId2 { get; set; }
        public string DriverName2 { get; set; }
        public string DriverIdCard2 { get; set; }
        public int CungDuongCode { get; set; }
        public string CungDuongName { get; set; }
        public string Romooc { get; set; }
        public string ShipNumber { get; set; }
        public string CompanyCode { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? StartAt { get; set; }
        public bool? Status { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
    }
    
}
