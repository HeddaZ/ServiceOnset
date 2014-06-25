using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ServiceOnset.Configuration
{
    public class ServiceOnsetConfig
    {
        public ServiceOnsetSettings Settings { get; set; }

        public ServiceOnsetConfig(XmlNode section)
        {

            this.Settings = new ServiceOnsetSettings();

            
        }
    }
}
