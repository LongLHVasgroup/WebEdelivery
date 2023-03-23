using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveVehicleRegisterOutDate
{
    public static class AppSetting
    {
        public static IConfiguration Configuration { get; set; }

        public static string ConnectionString { get; set; }

    }
}
