
using System.Collections.Generic;

namespace ProjectDiff
{
    public class StringsDiff
    {
        public List<string> Left;
        public List<string> Both;
        public List<string> Right;
    }

    public class TargetId
    {
        public TargetId(string path, string config, string platform)
        {
            Path = path;
            Target = new TargetDef()
            {
                Config = config,
                Platform = platform
            };
        }
        public TargetDef Target;
        public string Path;

        public override string ToString()
        {
            return Path + "(" + Target.Config + "|" + Target.Platform + ")";
        }
    }

    public class TargetDiff
    {
        public string Name;
        public TargetId Left;
        public TargetId Right;
        public Dictionary<string, StringsDiff> Diffs;
    }

    public class Diff
    {
        public List<TargetDiff> Targets;
    }
}
