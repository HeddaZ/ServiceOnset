using ServiceOnset.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ServiceOnset.Services
{
    public interface IService
    {
        IServiceStartInfo StartInfo { get; }
        Process InnerProcess { get; }

        void Start();
    }
}
