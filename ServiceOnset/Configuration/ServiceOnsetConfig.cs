using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace ServiceOnset.Configuration
{
    [DataContract]
    public class ServiceOnsetConfig
    {
        [DataMember(Name = "logPath")]
        public string LogPath { get; set; }

        [DataMember(Name = "services")]
        public ServiceOnsetService[] Services { get; set; }
    }

    [DataContract]
    public class ServiceOnsetService
    {
        [DataMember(Name = "command")]
        public string Command { get; set; }
        [DataMember(Name = "arguments")]
        public string Arguments { get; set; }
        [DataMember(Name = "initialDirectory")]
        public string InitialDirectory { get; set; }
    }
}
