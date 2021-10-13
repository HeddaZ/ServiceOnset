using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceOnset
{
    public class Logger
    {
        private const string RootLoggerName = "root";

        public ILog InnerLogger
        {
            get;
            private set;
        }
        public bool Enabled
        {
            get;
            set;
        }

        internal Logger(string name, bool enabled)
        {
            InnerLogger = LogManager.GetLogger(name);
            Enabled = enabled && AppHelper.Config.EnableLog;
        }
        internal Logger(bool enabled)
            : this(RootLoggerName, enabled)
        {
        }

        #region Logging

        public void Info(string format, params object[] args)
        {
            if (Enabled)
            {
                InnerLogger.InfoFormat(format, args);
            }
        }
        public void Info(string message)
        {
            if (Enabled)
            {
                InnerLogger.Info(message);
            }
        }
        public void Info(string message, Exception exception)
        {
            if (Enabled)
            {
                InnerLogger.Info(message, exception);
            }
        }

        public void Error(string format, params object[] args)
        {
            if (Enabled)
            {
                InnerLogger.ErrorFormat(format, args);
            }
        }
        public void Error(string message)
        {
            if (Enabled)
            {
                InnerLogger.Error(message);
            }
        }
        public void Error(string message, Exception exception)
        {
            if (Enabled)
            {
                InnerLogger.Error(message, exception);
            }
        }

        #endregion
    }
}
