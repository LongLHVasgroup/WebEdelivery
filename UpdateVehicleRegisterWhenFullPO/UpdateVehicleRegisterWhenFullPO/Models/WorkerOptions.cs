using System;
using System.Collections.Generic;
using System.Text;

namespace UpdateVehicleRegisterWhenFullPO.Models
{
    public class WorkerOptions
    {
        public string BanCanConnection { get; set; }
        public string WebConnection { get; set; }
    }

    public sealed class Config
    {
        #region App secrect

        public string appSecret;

        #endregion App secrect

        public string connWeb;
        public string connPMC;

        private static Config Instance = null;

        public static Config getInstance()
        {
            if (Instance == null)
            {
                Instance = new Config();
            }
            return Instance;
        }

        private Config()
        {
        }

        public string getAppSecret()
        {
            if (string.IsNullOrEmpty(appSecret))
            {
                appSecret = "HACK! PLEASE HACK PLEASE!";
            }
            return appSecret;
        }
    }
}