using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ServiceOnset.Configuration
{
    public class ServiceOnsetSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            ServiceOnsetConfig config = new ServiceOnsetConfig();

            #region appSettings

            //foreach (XmlAttribute xmlAttribute in section.Attributes)
            //{
            //    hashtable[xmlAttribute.Name] = xmlAttribute.Value;
            //}
            //return hashtable;

            

            #endregion

            return config;
        }

        private T Parse<T>(XmlNode node)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(new XmlNodeReader(node));
        }
    }
}
