using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class AuthenticateModelRespone : ActionMessage
    {
        public string token { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public Guid userID { get; set; }
        public int? role { get; set; }
        public string type { get; set; }
        public bool isService { get; set; }
    }
}