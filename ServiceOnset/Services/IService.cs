using log4net;
using ServiceOnset.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ServiceOnset.Services
{
    public interface IService : IDisposable
    {
        IServiceStartInfo StartInfo { get; }
        Process InnerProcess { get; }

        void Start();
    }
}
