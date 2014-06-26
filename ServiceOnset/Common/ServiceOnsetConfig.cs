using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;

namespace ServiceOnset.Common
{
    [DataContract]
    public class ServiceOnsetConfig
    {
        #region 静态方法

        public static ServiceOnsetConfig Create(string configPath, Encoding encoding)
        {
            string configString;
            using (StreamReader configReader = new StreamReader(configPath))
            {
                configString = configReader.ReadToEnd();
            }
            using (MemoryStream configStream = new MemoryStream(encoding.GetBytes(configString)))
            {
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(ServiceOnsetConfig));
                return (ServiceOnsetConfig)jsonSerializer.ReadObject(configStream);
            }
        }
        public static ServiceOnsetConfig Create(string configPath)
        {
            return ServiceOnsetConfig.Create(configPath, Encoding.UTF8);
        }

        #endregion

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
        [DataMember(Name = "runMode")]
        public ServiceRunModel RunMode { get; set; }
        [DataMember(Name = "intervalInSeconds")]
        public int IntervalInSeconds { get; set; }
    }

    [DataContract]
    public enum ServiceRunModel
    {
        [EnumMember]
        Daemon = 0,
        [EnumMember]
        Launch = 1,
        [EnumMember]
        Interval = 2
    }
}
