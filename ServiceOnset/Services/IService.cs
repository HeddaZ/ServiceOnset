using log4net;
using ServiceOnset.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ServiceOnset.Services
{
    public interface IService
    {
        Process InnerProcess { get; }
        IServiceStartInfo StartInfo { get; }
        ILog Logger { get; }

        void Start();
    }
}
