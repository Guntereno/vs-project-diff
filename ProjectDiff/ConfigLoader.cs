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

            XmlNode globalsNode = doc.SelectSingleNode("//Global");
            config.Globals = LoadGlobalConfig(globalsNode);

            Result = config;
        }

        private GlobalConfig LoadGlobalConfig(XmlNode globalsNode)
        {
            GlobalConfig global = new GlobalConfig();

            XmlNode ignoreNode = globalsNode.SelectSingleNode("//Ignore");
            if (ignoreNode != null)
            {
                string flags = ignoreNode.InnerText;
                global.Ignore = (IgnoreFlags)Enum.Parse(typeof(IgnoreFlags), flags);
            }

            XmlNode pathsNode = globalsNode.SelectSingleNode("//PathOperations");
            if (pathsNode != null)
            {
                string flags = pathsNode.InnerText;
                global.Paths = (PathOperations)Enum.Parse(typeof(PathOperations), flags);
            }

            return global;
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
