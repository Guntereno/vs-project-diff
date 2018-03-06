using System.Collections.Generic;

namespace ProjectDiff
{
    public class TargetDef
    {
        public string Config;
        public string Platform;

        public static bool operator == (TargetDef obj1, TargetDef obj2)
        {
            return (
                obj1.Config == obj2.Config &&
                obj1.Platform == obj2.Platform
            );
        }

        public static bool operator != (TargetDef obj1, TargetDef obj2)
        {
            return !(obj1 == obj2);
        }
    }

    class TargetModel
    {
        public TargetDef TargetDef;

        public string Configuration
        {
            get
            {
                return TargetDef.Config;
            }
        }

        public string Platform
        {
            get
            {
                return TargetDef.Platform;
            }
        }

        public List<string> PreprocessorDefinitions;
        public List<string> AdditionalIncludeDirectories;
        public List<string> AdditionalLibraryDirectories;
        public List<string> AdditionalDependencies;
        public List<string> Source;
        public List<string> Includes;
    }
}
