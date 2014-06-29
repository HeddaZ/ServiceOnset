using ServiceOnset.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceOnset.Services
{
    public class ServiceFactory
    {
        #region 单例

        private static ServiceFactory _instance;
        private static object _mutex = new object();
        public static ServiceFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_mutex)
                    {
                        if (_instance == null)
                        {
                            _instance = new ServiceFactory();
                        }
                    }
                }
                return _instance;
            }
        }
        private ServiceFactory()
        {
        }

        #endregion

        public IService Create(IServiceStartInfo startInfo)
        {
            switch (startInfo.RunMode)
            {
                case ServiceRunMode.Daemon:
                    return new DaemonService(startInfo) as IService;

                case ServiceRunMode.ForceInterval:
                case ServiceRunMode.Interval:
                case ServiceRunMode.Launch:
                default:
                    return new DaemonService(startInfo) as IService;
            }
        }
    }
}
