using log4net;
using ServiceOnset.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]

namespace ServiceOnset
{
    public class AppHelper
    {
        public static readonly string AppPath = Assembly.GetExecutingAssembly().Location;
        public static readonly string AppDirectory = Path.GetDirectoryName(AppPath);
        public static readonly string AppVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        #region 配置

        private static IServiceOnsetConfig _config;
        private static object _configMutex = new object();
        public static IServiceOnsetConfig Config
        {
            get
            {
                if (_config == null)
                {
                    lock (_configMutex)
                    {
                        if (_config == null)
                        {
                            _config = ServiceOnsetConfig.Create(AppHelper.AppPath + ".json"); // ServiceOnset.exe.json
                        }
                    }
                }
                return _config;
            }
        }

        #endregion

        #region 日志

        private static ILog _log;
        private static object _logMutex = new object();
        public static ILog Log
        {
            get
            {
                if (_log == null)
                {
                    lock (_logMutex)
                    {
                        if (_log == null)
                        {
                            _log = LogManager.GetLogger(typeof(AppHelper)); // root config node
                        }
                    }
                }
                return _log;
            }
        }

        #endregion
    }
}
