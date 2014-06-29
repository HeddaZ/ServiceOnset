using ServiceOnset.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ServiceOnset.Core
{
    public class ServiceHelper : IServiceHelper
    {
        public IServiceOnsetService StartInfo
        {
            get;
            private set;
        }

        public ServiceHelper(IServiceOnsetService startInfo)
        {
            this.StartInfo = startInfo;
        }

        public void Start()
        {
            
        }
    }
}
