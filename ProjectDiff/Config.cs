using System;
using System.Collections.Generic;

namespace ProjectDiff
{
    [Flags]
    public enum IgnoreFlags
    {
        Left = 1 << 0,
        Right = 1 << 1,
        Both = 1 << 2
    }

    [Flags]
    public enum PathOperations
    {
        MakeAbsolute = 1 << 0,
        ResolveMacros = 1 << 1,
        IgnoreCase = 1 << 2
    }

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

    public class GlobalConfig
    {
        public IgnoreFlags Ignore;
        public PathOperations Paths;
    }

    public class Config
    {
        public List<Mapping> Mappings;
        public GlobalConfig Globals;
    }
}
