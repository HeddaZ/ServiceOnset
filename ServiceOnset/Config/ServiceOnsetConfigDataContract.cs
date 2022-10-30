using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ServiceOnset.Config
{
    [DataContract]
    public partial class ServiceOnsetConfig
    {
        [DataMember(Name = "enableLog")]
        private bool _originalEnableLog;
        [DataMember(Name = "services")]
        private ServiceStartInfo[] _originalStartInfos;
    }

    [DataContract]
    public partial class ServiceStartInfo
    {
        [DataMember(Name = "disable")]
        private bool _originalDisable;
        [DataMember(Name = "name")]
        private string _originalName;
        [DataMember(Name = "desc")]
        private string _originalDescription;
        [DataMember(Name = "command")]
        private string _originalCommand;
        [DataMember(Name = "arguments")]
        private string _originalArguments;
        [DataMember(Name = "workingDirectory")]
        private string _originalWorkingDirectory;
        [DataMember(Name = "runMode")]
        private string _originalRunMode;
        [DataMember(Name = "intervalInSeconds")]
        private int _originalIntervalInSeconds;
        [DataMember(Name = "timingExp")]
        private string _originalTimingExp;
        [DataMember(Name = "useShellExecute")]
        private bool _originalUseShellExecute;
        [DataMember(Name = "hideWindow")]
        private bool _originalHideWindow;
        [DataMember(Name = "killExistingProcess")]
        private bool _originalKillExistingProcess;
        [DataMember(Name = "enableLog")]
        private bool _originalEnableLog;
    }
}
