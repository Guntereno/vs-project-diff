using System.Collections.Generic;

namespace ProjectDiff
{
    public class Target
    {
        public string ProjectPath;
        public string Config;
        public string Platform;
    }

    public class Mapping
    {
        public string Name;
        public Target Left;
        public Target Right;
    }

    public class Config
    {
        public List<Mapping> Mappings;
    }
}
