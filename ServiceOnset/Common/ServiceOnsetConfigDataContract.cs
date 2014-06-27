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
        [DataMember(Name = "logPath")]
        private string _logPath;
        [DataMember(Name = "services")]
        private ServiceOnsetService[] _services;
    }

    [DataContract]
    public partial class ServiceOnsetService
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
    }
}
