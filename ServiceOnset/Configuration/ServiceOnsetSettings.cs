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
            if (!string.IsNullOrWhiteSpace(  binder.Name)){
                string normalizedElementName = binder.Name.Substring(0, 1).ToLower() + binder.Name.Substring(1);

                XmlNode node = this.Section.ChildNodes.OfType<XmlNode>()
                    .FirstOrDefault(n => n.NodeType == XmlNodeType.Element && n.Name == normalizedElementName);
                result = null;
                return true;
            }
            else
            {
                result=null;
                return false;
            }
        }
    }
}
