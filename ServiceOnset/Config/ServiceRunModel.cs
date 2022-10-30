using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceOnset.Config
{
    public enum ServiceRunMode : int
    {
        Daemon = 1,
        Launch = 2,
        Interval = 4,
        Timing = 8
    }
}
