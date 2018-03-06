using System.Collections.Generic;

namespace ProjectDiff
{
    class TargetModel
    {
        public string Configuration;
        public string Platform;

        public List<string> PreprocessorDefinitions;
        public List<string> AdditionalIncludeDirectories;
        public List<string> AdditionalLibraryDirectories;
        public List<string> AdditionalDependencies;
    }
}
