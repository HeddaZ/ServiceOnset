using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceOnset.Config
{
    public enum ServiceRunMode
    {
        Daemon,
        Launch,
        Interval,
        ForceInterval
    }
}
