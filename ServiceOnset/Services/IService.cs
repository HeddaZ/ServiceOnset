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
        Process InternalProcess { get; }
        IServiceStartInfo StartInfo { get; }

        void Start();
    }
}
