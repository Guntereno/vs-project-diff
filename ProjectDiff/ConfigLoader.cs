using System;
using System.Collections.Generic;
using System.Xml;

namespace ProjectDiff
{
    class ConfigLoader
    {
        public Config Result
        {
            get;
            private set;
        }

        public ConfigLoader(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            var config = new Config();

            var mappingNodes = doc.SelectNodes("//Mapping");
            if(mappingNodes != null)
            {
                config.Mappings = new List<Mapping>();
                int mappingCount = mappingNodes.Count;
                for (int i = 0; i < mappingCount; ++i)
                {
                    XmlNode mappingNode = mappingNodes[i];
                    config.Mappings.Add(LoadMapping(mappingNode));
                }
            }

            XmlNode ignoreNode = doc.SelectSingleNode("//Global/Ignore");
            if(ignoreNode != null)
            {
                string flags = ignoreNode.InnerText;
                config.Ignore = (IgnoreFlags)Enum.Parse(typeof(IgnoreFlags), flags);
            }

            Result = config;
        }

        private Mapping LoadMapping(XmlNode mappingNode)
        {
            var mapping = new Mapping();
            mapping.Name = mappingNode.Attributes["Name"].Value;
            mapping.Left = LoadTarget(mappingNode.SelectSingleNode("Left"));
            mapping.Right = LoadTarget(mappingNode.SelectSingleNode("Right"));
            return mapping;
        }
        private Target LoadTarget(XmlNode xmlNode)
        {
            var target = new Target();
            target.ProjectPath = xmlNode.Attributes["ProjectPath"].Value;
            target.Config = xmlNode.Attributes["Config"].Value;
            target.Platform = xmlNode.Attributes["Platform"].Value;
            return target;
        }
    }
}
