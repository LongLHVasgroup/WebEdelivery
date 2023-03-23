using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UpdateVehicleRegisterWhenFullPO.Models.WebModels
{
    public class RegisterForShip

    {
        [Key]
        public int Id { get; set; }

        public string PONumber { get; set; }
        public string DVVCCode { get; set; }
        public string DVVC { get; set; }
        public string VehicleNumber { get; set; }
        public string DriverName1 { get; set; }
        public string DriverIdCard1 { get; set; }
        public string DriverName2 { get; set; }
        public string DriverIdCard2 { get; set; }
        public int CungDuongCode { get; set; }
        public string CungDuongName { get; set; }
        public string Romooc { get; set; }
        public string ShipNumber { get; set; }
        public string CompanyCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime StartAt { get; set; }
        public bool Status { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
    }
}