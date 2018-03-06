using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace ProjectDiff
{
    class ProjectLoader
    {
        public ProjectFileModel Result
        {
            get;
            private set;
        }

        private GlobalConfig _globalConfig;
        private XmlNamespaceManager _nsMan;
        private FileInfo _projectFileInfo;
        private ProjectFileModel _result;

        private delegate string ProcessString(string input);

        public ProjectLoader(string path, GlobalConfig globalConfig)
        {
            _globalConfig = globalConfig;

            _projectFileInfo = new FileInfo(path);

            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            _nsMan = new XmlNamespaceManager(doc.NameTable);
            const string kMsBuildNs = "http://schemas.microsoft.com/developer/msbuild/2003";
            _nsMan.AddNamespace("msb", kMsBuildNs);

            _result = new ProjectFileModel();

            _result.Path = path;

            var globalsNode = doc.SelectSingleNode("//msb:Project/msb:PropertyGroup[@Label='Globals']", _nsMan);
            if (globalsNode != null)
            {
                _result.Globals = new Dictionary<string, string>();
                for (int i = 0; i < globalsNode.ChildNodes.Count; i++)
                {
                    XmlNode childNode = globalsNode.ChildNodes[i];
                    _result.Globals[childNode.Name] = globalsNode.ChildNodes[i].InnerText;
                }
            }

            var itemDefinitionGroups = doc.SelectNodes("//msb:Project/msb:ItemDefinitionGroup", _nsMan);
            if (itemDefinitionGroups != null)
            {
                _result.Targets = new List<TargetModel>();
                for (int i = 0; i < itemDefinitionGroups.Count; i++)
                {
                    XmlNode childNode = itemDefinitionGroups[i];
                    _result.Targets.Add(LoadTarget(childNode));
                }
            }

            Result = _result;
        }

        private string ResolveFilePath(string input)
        {
            if((_globalConfig.Paths & PathOperations.ResolveMacros) != 0)
            {
                input = ResolveMacros(input);
            }

            if ((_globalConfig.Paths & PathOperations.MakeAbsolute) != 0)
            {
                input = BuildAbsolutePath(input);
            }

            return input;
        }

        private string ResolveMacros(string input)
        {
            const string kMacroPattern = @"\$\((.*?)\)";
            var regex = new Regex(kMacroPattern);
            Match match = regex.Match(input);
            if (match.Success)
            {
                var macros = new List<string>();
                bool replacementsMade = false;
                for (int i=1; i<match.Groups.Count; ++i)
                {
                    string macro = match.Groups[i].Value;
                    if (_result.Globals.ContainsKey(macro))
                    {
                        string replacement = _result.Globals[macro];
                        input = input.Replace("$(" + macro + ")", replacement);
                        replacementsMade = true;
                    }
                }
                if (replacementsMade)
                {
                    return ResolveMacros(input);
                }
                else
                {
                    return input;
                }
            }
            else
            {
                return input;
            }
        }

        private string BuildAbsolutePath(string input)
        {
            string fullPath = Path.Combine(_projectFileInfo.Directory.FullName, input);
            string absPath = Path.GetFullPath(new Uri(fullPath).LocalPath);
            return absPath;
        }

        private TargetModel LoadTarget(XmlNode itemDefGroupNode)
        {
            TargetModel result = new TargetModel();

            string condition = itemDefGroupNode.Attributes["Condition"].InnerText;

            // Get the configuration and platform from the condition
            const string kPattern = @"^'\$\(Configuration\)\|\$\(Platform\)'=='(.*)\|(.*)'$";
            var regex = new Regex(kPattern);
            Match match = regex.Match(condition);
            if (match.Success && (match.Groups.Count == 3))
            {
                result.Configuration = match.Groups[1].Value;
                result.Platform = match.Groups[2].Value;
            }
            else
            {
                throw new Exception("Invalid Condition in ItemDefinitionGroup!: " + condition);
            }

            result.PreprocessorDefinitions = BuildListFromNode(itemDefGroupNode, "msb:ClCompile/msb:PreprocessorDefinitions");
            result.AdditionalIncludeDirectories = BuildListFromNode(
                itemDefGroupNode,
                "msb:ClCompile/msb:AdditionalIncludeDirectories",
                ResolveFilePath);
            result.AdditionalLibraryDirectories = BuildListFromNode(
                itemDefGroupNode,
                "msb:Link/msb:AdditionalLibraryDirectories",
                ResolveFilePath);
            result.AdditionalDependencies = BuildListFromNode(itemDefGroupNode, "msb:Link/msb:AdditionalDependencies");

            return result;
        }

        private List<string> BuildListFromNode(XmlNode itemDefGroupNode, String xPath, ProcessString process = null)
        {
            var node = itemDefGroupNode.SelectSingleNode(xPath, _nsMan);
            if(node != null)
            {
                var list = new List<string>(node.InnerText.Split(';'));
                if(process != null)
                {
                    list = list.Select(i => process(i)).ToList();
                }
                return list;
            }
            else
            {
                return null;
            }
        }
    }
}
