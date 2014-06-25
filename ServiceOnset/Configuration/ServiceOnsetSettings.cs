using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ServiceOnset.Configuration
{
    public class ServiceOnsetSettings : DynamicObject
    {
        public XmlNode Section { get; set; }

        public ServiceOnsetSettings(XmlNode settingsSection)
        {
            this.Section = settingsSection;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            
            
        }
    }
}
