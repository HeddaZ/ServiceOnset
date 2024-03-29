﻿using ServiceOnset.Config;
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
        public const string AppTitle = "ServiceOnset";
        public const string AppDescription = "Run multiple programs in background upon single windows service. (QQ:9812152)";

        public static readonly string AppPath = Assembly.GetExecutingAssembly().Location;
        public static readonly string AppDirectory = Path.GetDirectoryName(AppPath);
        public static readonly string AppVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        #region Log singleton

        private static Logger _log;
        private static object _logMutex = new object();
        public static Logger Log
        {
            get
            {
                if (_log == null)
                {
                    lock (_logMutex)
                    {
                        if (_log == null)
                        {
                            _log = new Logger(Config.EnableLog);
                        }
                    }
                }
                return _log;
            }
        }

        #endregion
        #region Config singleton

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
