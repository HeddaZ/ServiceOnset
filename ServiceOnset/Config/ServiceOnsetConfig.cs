using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;

namespace ServiceOnset.Config
{
    public interface IServiceOnsetConfig
    {
        bool EnableLog { get; }
        IServiceStartInfo[] StartInfos { get; }
    }
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

        private bool? _enableLog;
        public bool EnableLog
        {
            get
            {
                if (!_enableLog.HasValue)
                {
                    _enableLog = _originalEnableLog;
                }
                return _enableLog.Value;
            }
        }

        private IServiceStartInfo[] _startInfos;
        public IServiceStartInfo[] StartInfos
        {
            get
            {
                if (_startInfos == null)
                {
                    if (_originalStartInfos == null)
                    {
                        throw new ArgumentNullException("services");
                    }
                    else
                    {
                        _startInfos = _originalStartInfos.OfType<IServiceStartInfo>().ToArray();
                    }
                }
                return _startInfos;
            }
        }
    }

    public interface IServiceStartInfo
    {
        bool Disable { get; }
        string Name { get; }
        string Description { get; }
        string Command { get; }
        string Arguments { get; }
        string WorkingDirectory { get; }
        ServiceRunMode RunMode { get; }
        int IntervalInSeconds { get; }
        string TimingExp { get; }
        bool UseShellExecute { get; }
        bool HideWindow { get; }
        bool KillExistingProcess { get; }
        bool EnableLog { get; }
    }
    public partial class ServiceStartInfo : IServiceStartInfo
    {
        private bool? _disable;
        public bool Disable
        {
            get
            {
                if (!_disable.HasValue)
                {
                    _disable = _originalDisable;
                }
                return _disable.Value;
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                if (_name == null)
                {
                    if (string.IsNullOrWhiteSpace(_originalName))
                    {
                        throw new ArgumentNullException("name");
                    }
                    else
                    {
                        _name = _originalName;
                    }
                }
                return _name;
            }
        }

        private string _description;
        public string Description
        {
            get
            {
                if (_description == null)
                {
                    _description = _originalDescription;
                }
                return _description;
            }
        }

        private string _command;
        public string Command
        {
            get
            {
                if (_command == null)
                {
                    if (string.IsNullOrWhiteSpace(_originalCommand))
                    {
                        throw new ArgumentNullException("command");
                    }
                    else if (Path.IsPathRooted(_originalCommand))
                    {
                        _command = _originalCommand;
                    }
                    else
                    {
                        #region 非完整路径，尝试匹配程序路径、工作目录、系统路径

                        string possibleCommand = Path.Combine(AppHelper.AppDirectory, _originalCommand);
                        if (CommandExists(possibleCommand))
                        {
                            _command = possibleCommand;
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(_originalWorkingDirectory))
                            {
                                _command = _originalCommand;
                            }
                            else
                            {
                                possibleCommand = Path.IsPathRooted(_originalWorkingDirectory)
                                    ? Path.Combine(_originalWorkingDirectory, _originalCommand)
                                    : Path.Combine(AppHelper.AppDirectory, _originalWorkingDirectory, _originalCommand);
                                _command = CommandExists(possibleCommand)
                                    ? possibleCommand
                                    : _originalCommand;
                            }
                        }

                        #endregion                        
                    }
                }
                return _command;
            }
        }

        private string _arguments;
        public string Arguments
        {
            get
            {
                if (_arguments == null)
                {
                    _arguments = _originalArguments ?? string.Empty;
                }
                return _arguments;
            }
        }

        private string _workingDirectory;
        public string WorkingDirectory
        {
            get
            {
                if (_workingDirectory == null)
                {
                    _workingDirectory = string.IsNullOrWhiteSpace(_originalWorkingDirectory)
                        ? Path.GetDirectoryName(Command)
                        : (Path.IsPathRooted(_originalWorkingDirectory)
                            ? _originalWorkingDirectory
                            : Path.Combine(AppHelper.AppDirectory, _originalWorkingDirectory));
                }
                return _workingDirectory;
            }
        }

        private ServiceRunMode? _runMode;
        public ServiceRunMode RunMode
        {
            get
            {
                if (!_runMode.HasValue)
                {
                    if (int.TryParse(_originalRunMode, out int runModeValue))
                    {
                        _runMode = (ServiceRunMode)runModeValue;
                    }
                    else if (Enum.TryParse(_originalRunMode, true, out ServiceRunMode runModeEnum))
                    {
                        _runMode = runModeEnum;
                    }
                    else
                    {
                        _runMode = ServiceRunMode.Daemon;
                    }
                }
                return _runMode.Value;
            }
        }

        private int? _intervalInSeconds;
        public int IntervalInSeconds
        {
            get
            {
                if (!_intervalInSeconds.HasValue)
                {
                    _intervalInSeconds = _originalIntervalInSeconds <= 0
                        ? 30
                        : _originalIntervalInSeconds;
                }
                return _intervalInSeconds.Value;
            }
        }

        private string _timingExp;
        public string TimingExp
        {
            get
            {
                if (_timingExp == null)
                {
                    _timingExp = string.IsNullOrWhiteSpace(_originalTimingExp)
                        ? "00"
                        : _originalTimingExp;
                }
                return _timingExp;
            }
        }

        private bool? _useShellExecute;
        public bool UseShellExecute
        {
            get
            {
                if (!_useShellExecute.HasValue)
                {
                    _useShellExecute = _originalUseShellExecute;
                }
                return _useShellExecute.Value;
            }
        }

        private bool? _hideWindow;
        public bool HideWindow
        {
            get
            {
                if (!_hideWindow.HasValue)
                {
                    _hideWindow = _originalHideWindow;
                }
                return _hideWindow.Value;
            }
        }

        private bool? _killExistingProcess;
        public bool KillExistingProcess
        {
            get
            {
                if (!_killExistingProcess.HasValue)
                {
                    _killExistingProcess = _originalKillExistingProcess;
                }
                return _killExistingProcess.Value;
            }
        }

        private bool? _enableLog;
        public bool EnableLog
        {
            get
            {
                if (!_enableLog.HasValue)
                {
                    _enableLog = _originalEnableLog;
                }
                return _enableLog.Value;
            }
        }

        private bool CommandExists(string command)
        {
            if (File.Exists(command))
            {
                return true;
            }

            string directory = Path.GetDirectoryName(command);
            if (Directory.Exists(directory))
            {
                string fileName = Path.GetFileNameWithoutExtension(command);
                return Directory.GetFiles(directory, fileName + ".*", SearchOption.TopDirectoryOnly).Length > 0;
            }
            else
            {
                return false;
            }
        }
    }
}
