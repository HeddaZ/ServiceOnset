using ServiceOnset.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ServiceOnset
{
    public class AppHelper
    {
        public static readonly string AppPath = Assembly.GetExecutingAssembly().Location;
        public static readonly string AppDirectory = Path.GetDirectoryName(AppPath);
        public static readonly string AppVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        #region 配置单例

        private static ServiceOnsetConfig _config;
        private static object _configMutex = new object();
        public static ServiceOnsetConfig Config
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
