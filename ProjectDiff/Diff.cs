
using System.Collections.Generic;

namespace ProjectDiff
{
    public class StringsDiff
    {
        public List<string> Left;
        public List<string> Both;
        public List<string> Right;
    }

    public class TargetDiff
    {
        public string Name;
        public Dictionary<string, StringsDiff> Diffs;
    }

    public class Diff
    {
        public List<TargetDiff> Targets;
    }
}
