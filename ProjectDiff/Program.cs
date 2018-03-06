using System.Diagnostics;
using System.IO;

namespace ProjectDiff
{
    class Program
    {
        static int Main(string[] args)
        {
            if(args.Length != 1)
            {
                Debug.WriteLine("usage: ProjecDif config.xml");
                return 1;
            }

            string path = args[0];

            // Set the working directory to the one containing the config file
            var configFileInfo = new FileInfo(path);
            Directory.SetCurrentDirectory(configFileInfo.Directory.FullName);

            var config = new ConfigLoader(path);
            var diff = new DiffProcessor(config.Result);
            var diffLogger = new DiffLogger(diff.Result);

            return 0;
        }
    }
}
