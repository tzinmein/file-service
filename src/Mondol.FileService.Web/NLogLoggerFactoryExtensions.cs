using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mondol.WebPlatform;
using NLog.Extensions.Logging;
using System;
using System.IO;

namespace Mondol.Extensions.Logging
{
    public static class NLogLoggerFactoryExtensions
    {
        public static void AddNLog(this ILoggingBuilder logBuilder, IWebHostEnvironment env, IConfiguration config)
        {
            logBuilder.Services.AddSingleton<ILoggerProvider, NLogLoggerProvider>();
            LoadLogManager(env, config);
        }

        [Obsolete]
        public static void AddNLog(this ILoggerFactory logFac, IWebHostEnvironment env, IConfiguration config)
        {
            logFac.AddNLog();

            LoadLogManager(env, config);
        }

        private static void LoadLogManager(IWebHostEnvironment env, IConfiguration config)
        {
            /*ConfigureNLog()会忽略异常，此处手动设置*/
            //env.ConfigureNLog("nlog.config");

            var nlogCfgFile = Path.Combine(env.GetConfigPath(), $"nlog.{env.EnvironmentName}.config");
            if (!File.Exists(nlogCfgFile))
                nlogCfgFile = Path.Combine(env.GetConfigPath(), "nlog.config");

            var logsDir = config["Logging:LogsDir"];
            if (string.IsNullOrEmpty(logsDir))
                logsDir = Path.Combine(env.ContentRootPath, "logs");

            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(nlogCfgFile);
            NLog.LogManager.Configuration.Variables["logs-dir"] = logsDir;
        }
    }
}
