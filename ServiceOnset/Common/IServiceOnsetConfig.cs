using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceOnset.Common
{
    public interface IServiceOnsetConfig
    {
        string LogPath { get; }
        IServiceOnsetService[] Services { get; }
    }

    public interface IServiceOnsetService
    {
        string Command { get; }
        string Arguments { get; }
        string InitialDirectory { get; }
        ServiceRunModel RunMode { get; }
        int IntervalInSeconds { get; }
    }
}
