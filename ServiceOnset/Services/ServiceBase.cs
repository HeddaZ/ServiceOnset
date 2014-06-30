using log4net;
using ServiceOnset.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ServiceOnset.Services
{
    public abstract class ServiceBase : IService
    {
        public Process InnerProcess
        {
            get;
            private set;
        }
        public IServiceStartInfo StartInfo
        {
            get;
            private set;
        }
        public ILog Logger
        {
            get
            {
                return LogManager.GetLogger(this.StartInfo.Name);
            }
        }

        public ServiceBase(IServiceStartInfo startInfo)
        {
            this.StartInfo = startInfo;
            this.InnerProcess = new Process();
        }

        public abstract void Start();
    }
}
