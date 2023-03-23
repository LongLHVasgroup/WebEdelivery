using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSyncPoToWeb.repositories
{
    public class SAPRepository
    {
        private RfcDestination GetRfcDestination(string name
          , string username, string password, string client
          , string language, string appServerHost, string systemNumber
          , string maxPoolSize, string idleTimeout, string sapRouter)
        {
            RfcConfigParameters parameters = new RfcConfigParameters();
            //name = "asdasd";
            parameters.Add(RfcConfigParameters.Name, name);
            parameters.Add(RfcConfigParameters.User, username);
            parameters.Add(RfcConfigParameters.Password, password);
            parameters.Add(RfcConfigParameters.Client, client);
            parameters.Add(RfcConfigParameters.Language, language);
            parameters.Add(RfcConfigParameters.AppServerHost, appServerHost);
            parameters.Add(RfcConfigParameters.SystemNumber, systemNumber);
            parameters.Add(RfcConfigParameters.MaxPoolSize, maxPoolSize);
            parameters.Add(RfcConfigParameters.IdleTimeout, idleTimeout);
            parameters.Add(RfcConfigParameters.SAPRouter, sapRouter);
            

            return RfcDestinationManager.GetDestination(parameters);
        }

        public RfcDestination GetRfcWithConfig()
        {

            //return GetRfcDestination("ahtdeva"
            //             , "ahtsupport", "$htsupport300", "300", "EN"
            //             , "192.168.100.37", "00", "20", "10", "/H/113.161.67.226/S/3299/H/");
            /*
            return GetRfcDestination(
                //name
                          "erpprd"
                //username // password
                         , "ahtsupport", "$htsupport800"
                //client // language
                         , "800", "EN"
                // app serverHost
                         , "192.168.100.40"
                // system number
                         , "01"
                //max PoolSize
                         , "20"
                //idle Timeout
                         , "10", "");
                         //sapRouter
                         //, "/H/192.168.100.40/S/3299");//??
            //nãy nói là 192.168.100.35 => SAP router
            
            */
            ///H/113.161.67.226/
            return GetRfcDestination(
                          ConfigUtilities.GetSysConfigAppSetting("SAPname"),
                          ConfigUtilities.GetSysConfigAppSetting("SAPusername"),
                          ConfigUtilities.GetSysConfigAppSetting("SAPpassword"),
                          ConfigUtilities.GetSysConfigAppSetting("SAPclient"),
                          ConfigUtilities.GetSysConfigAppSetting("SAPlanguage"),
                          ConfigUtilities.GetSysConfigAppSetting("SAPappServerHost"),
                          ConfigUtilities.GetSysConfigAppSetting("SAPsystemNumber"),
                          ConfigUtilities.GetSysConfigAppSetting("SAPmaxPoolSize"),
                          ConfigUtilities.GetSysConfigAppSetting("SAPidleTimeout"),
                          ConfigUtilities.GetSysConfigAppSetting("SAPsapRouter")
                          );
            

            //return GetRfcDestination("ahtdev", "NZZ.IT01", "NZZ.IT01", "400", "EN", "192.168.100.76", "00", "20", "10", "");

        }
    }
}
