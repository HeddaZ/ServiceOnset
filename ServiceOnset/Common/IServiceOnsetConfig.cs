using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceOnset.Common
{
    public interface IServiceOnsetConfig
    {
        IServiceOnsetService[] Services { get; }
    }

    public interface IServiceOnsetService
    {
        string Command { get; }
        string Arguments { get; }
        string InitialDirectory { get; }
        ServiceRunMode RunMode { get; }
        int IntervalInSeconds { get; }
    }
}
