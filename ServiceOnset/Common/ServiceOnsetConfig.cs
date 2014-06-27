using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;

namespace ServiceOnset.Common
{
    public partial class ServiceOnsetConfig : IServiceOnsetConfig
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

        public string LogPath
        {
            get
            {
                return !string.IsNullOrWhiteSpace(_logPath) ? _logPath : AppHelper.AppDirectory;
            }
        }
        public IServiceOnsetService[] Services
        {
            get
            {
                return _services;
            }
        }
    }

    public partial class ServiceOnsetService : IServiceOnsetService
    {
        public string Command
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_command))
                {
                    throw new Exception("Invalid command!");
                }
                return _command;
            }
        }
        public string Arguments
        {
            get
            {
                return _arguments;
            }
        }
        public string InitialDirectory
        {
            get
            {
                return !string.IsNullOrWhiteSpace(_initialDirectory) ? _initialDirectory : Path.GetDirectoryName(this.Command);
            }
        }
        public ServiceRunModel RunMode
        {
            get
            {
                return !string.IsNullOrWhiteSpace(_runMode) ? (ServiceRunModel)Enum.Parse(typeof(ServiceRunModel), _runMode, true) : ServiceRunModel.Daemon;
            }
        }
        public int IntervalInSeconds
        {
            get
            {
                return _intervalInSeconds > 0 ? _intervalInSeconds : 60;
            }
        }
    }
}
