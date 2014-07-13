using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;

namespace ServiceOnset.Config
{
    public partial class ServiceOnsetConfig : IServiceOnsetConfig
    {
        #region Creator

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

        private ServiceOnsetConfig()
        {
        }

        public bool EnableLog
        {
            get
            {
                return _enableLog;
            }
        }
        public IServiceStartInfo[] StartInfos
        {
            get
            {
                return _startInfos.OfType<IServiceStartInfo>().ToArray();
            }
        }
    }

    public partial class ServiceStartInfo : IServiceStartInfo
    {
        public string Name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_name))
                {
                    throw new ArgumentNullException("name");
                }
                return _name;
            }
        }
        public string Command
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_command))
                {
                    throw new ArgumentNullException("command");
                }
                return _command;
            }
        }
        public string Arguments
        {
            get
            {
                return _arguments != null ? _arguments : string.Empty;
            }
        }
        public string WorkingDirectory
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_workingDirectory))
                {
                    _workingDirectory = Path.GetDirectoryName(this.Command);
                }
                if (string.IsNullOrWhiteSpace(_workingDirectory))
                {
                    _workingDirectory = AppHelper.AppDirectory;
                }
                return _workingDirectory;
            }
        }
        public ServiceRunMode RunMode
        {
            get
            {
                return !string.IsNullOrWhiteSpace(_runMode) ? (ServiceRunMode)Enum.Parse(typeof(ServiceRunMode), _runMode, true) : ServiceRunMode.Daemon;
            }
        }
        public int IntervalInSeconds
        {
            get
            {
                return _intervalInSeconds > 0 ? _intervalInSeconds : 30;
            }
        }
        public bool UseShellExecute
        {
            get
            {
                return _useShellExecute;
            }
        }
        public bool EnableLog
        {
            get
            {
                return _enableLog;
            }
        }
    }
}
