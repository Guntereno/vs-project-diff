using System;
using System.Collections.Generic;
using System.Xml;

namespace ProjectDiff
{
    class ProjectFileModel
    {
        public string Path;
        public Dictionary<string, string> Globals;
        public List<TargetModel> Targets;

        public TargetModel FindTarget(string config, string platform)
        {
            foreach(var target in Targets)
            {
                if((target.Configuration == config) && (target.Platform == platform))
                {
                    return target;
                }
            }

            return null;
        }
    }
}
