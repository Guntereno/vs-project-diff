using System.IO;

namespace ProjectDiff
{
    class Program
    {
        static void Main(string[] args)
        {
            const string kConfigPath = "C:\\LyFactorGit\\rfactorWrapper\\LyFactor.Orbis\\ProjectDiffConfig.xml";

            // Set the working directory to the one containing the config file
            var configFileInfo = new FileInfo(kConfigPath);
            Directory.SetCurrentDirectory(configFileInfo.Directory.FullName);

            var config = new ConfigLoader(kConfigPath);
            var diff = new DiffProcessor(config.Result);
            var diffLogger = new DiffLogger(diff.Result, true);
        }
    }
}
