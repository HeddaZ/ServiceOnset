using ServiceOnset.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ServiceOnset.Services
{
    public abstract class ServiceBase : IService
    {
        public Process InternalProcess
        {
            get;
            private set;
        }
        public IServiceStartInfo StartInfo
        {
            get;
            private set;
        }

        public ServiceBase(IServiceStartInfo startInfo)
        {
            this.StartInfo = startInfo;
            this.InternalProcess = new Process();
        }

        public abstract void Start();
    }
}
