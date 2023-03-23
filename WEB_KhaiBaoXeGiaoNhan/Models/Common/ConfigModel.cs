using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Common
{
    public sealed class Config
    {
        #region App secrect

        public string appSecret;

        #endregion App secrect

        public string connWeb;
        public string connPMC;
        public string connPMC3000;
        public string connPMC6000;
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