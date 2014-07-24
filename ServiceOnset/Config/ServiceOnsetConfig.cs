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
                if (_startInfos == null)
                {
                    throw new ArgumentNullException("services");
                }
                return _startInfos.OfType<IServiceStartInfo>().ToArray();
            }
        }
    }

    public partial class ServiceStartInfo : IServiceStartInfo
    {
        private ServiceRunMode? _runMode;

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
                if (_arguments == null)
                {
                    _arguments = string.Empty;
                }
                return _arguments;
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
                if (!_runMode.HasValue)
                {
                    ServiceRunMode value;
                    if (Enum.TryParse<ServiceRunMode>(_runModeText, true, out value))
                    {
                        _runMode = value;
                    }
                    else
                    {
                        _runMode = ServiceRunMode.Daemon;
                    }
                }
                return _runMode.Value;
            }
        }
        public int IntervalInSeconds
        {
            get
            {
                if (_intervalInSeconds <= 0)
                {
                    _intervalInSeconds = 30;
                }
                return _intervalInSeconds;
            }
        }
        public bool UseShellExecute
        {
            get
            {
                return _useShellExecute;
            }
        }
        public bool AllowWindow
        {
            get
            {
                return _allowWindow;
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
