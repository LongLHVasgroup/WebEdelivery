using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB_KhaiBaoXeGiaoNhan.Models
{
    public partial class VehicleUpdateModel
    {
        private Guid driverId;
        private decimal trongLuongDuKien;
        private string assets;
        private string orderNumber;
        private bool isActive;
        private List<string> vatTuUpdate;

        public Guid DriverId { get => driverId; set => driverId = value; }
        public decimal TrongLuong { get => trongLuongDuKien; set => trongLuongDuKien = value; }
        public string Assets { get => assets; set => assets = value; }
        public List<string> VatTuUpdate { get => vatTuUpdate; set => vatTuUpdate = value; }
        public string OrderNumber { get => orderNumber; set => orderNumber = value; }
        public bool IsActive { get => isActive; set => isActive = value; }
    }
}