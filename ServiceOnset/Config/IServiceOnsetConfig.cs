using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceOnset.Config
{
    public interface IServiceOnsetConfig
    {
        bool EnableLog { get; }
        IServiceStartInfo[] StartInfos { get; }
    }

    public interface IServiceStartInfo
    {
        string Name { get; }
        string Command { get; }
        string Arguments { get; }
        string WorkingDirectory { get; }
        ServiceRunMode RunMode { get; }
        int IntervalInSeconds { get; }
        bool EnableLog { get; }
    }
}
