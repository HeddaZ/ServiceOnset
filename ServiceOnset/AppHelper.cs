using log4net;
using ServiceOnset.Config;
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

        public static ILog AppLogger
        {
            get
            {
                return LogManager.GetLogger("AppLogger");
            }
        }

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
    }
}
