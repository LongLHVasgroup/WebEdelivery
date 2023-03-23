using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WEB_KhaiBaoXeGiaoNhan
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region log

            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead("log4netcore.config"));
            var repo = log4net.LogManager.CreateRepository(Assembly.GetEntryAssembly(),
                       typeof(log4net.Repository.Hierarchy.Hierarchy));
            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
            log4net.ILog log = log4net.LogManager.GetLogger(typeof(Program));
            log.Info("------------------START WEBSITE---------------------");

            #endregion log

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}