using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ServiceOnset.Common
{
    [DataContract]
    public partial class ServiceOnsetConfig
    {
        [DataMember(Name = "services")]
        private ServiceStartInfo[] _starInfos;
    }

    [DataContract]
    public partial class ServiceStartInfo
    {
        [DataMember(Name = "command")]
        private string _command;
        [DataMember(Name = "arguments")]
        private string _arguments;
        [DataMember(Name = "initialDirectory")]
        private string _initialDirectory;
        [DataMember(Name = "runMode")]
        private string _runMode;
        [DataMember(Name = "intervalInSeconds")]
        private int _intervalInSeconds;
        [DataMember(Name = "useShellExecute")]
        private bool _useShellExecute;
    }
}
