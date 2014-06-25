using ServiceOnset.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ServiceOnset
{
    public class AppHelper
    {
        public static readonly string AppPath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');

        private static ServiceOnsetConfig _config;
        private static object _configMutex = new object();
        public static ServiceOnsetConfig Config
        {
            get
            {
                if (_config == null)
                {
                    lock (_configMutex)
                    {
                        if (_config == null)
                        {
                            string configString;
                            using (StreamReader configReader = new StreamReader(AppPath + "\\ServiceOnset.json"))
                            {
                                configString = configReader.ReadToEnd();
                            }
                            using (MemoryStream configStream = new MemoryStream(Encoding.UTF8.GetBytes(configString)))
                            {
                                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ServiceOnsetConfig));
                                _config = (ServiceOnsetConfig)serializer.ReadObject(configStream);
                            }
                        }
                    }
                }
                return _config;
            }
        }
    }
}
