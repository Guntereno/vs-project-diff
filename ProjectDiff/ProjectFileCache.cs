using System.Collections.Generic;
using System.IO;

namespace ProjectDiff
{
    class ProjectFileCache
    {
        private GlobalConfig _globalConfig;

        public ProjectFileCache(GlobalConfig globalConfig)
        {
            _projectDict = new Dictionary<string, ProjectFileModel>();
            _globalConfig = globalConfig;
        }

        public ProjectFileModel this[string path]
        {
            get
            {
                string absPath = Path.GetFullPath(path);
                if(!_projectDict.ContainsKey(absPath))
                {
                    var loader = new ProjectLoader(absPath, _globalConfig);
                    _projectDict.Add(absPath, loader.Result);
                }
                return _projectDict[absPath];
            }
        }

        Dictionary<string, ProjectFileModel> _projectDict;
    }
}
