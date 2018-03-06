using System;
using System.Collections.Generic;

namespace ProjectDiff
{
    class DiffLogger
    {
        public DiffLogger(Diff diff)
        {
            OutputDiff(diff);
        }

        private void OutputHeader(int level, string text)
        {
            Console.WriteLine(new String('#', level) + " " + text + "\n");
        }

        private void OutputList(List<string> list)
        {
            if ((list != null) && (list.Count > 0))
            {
                foreach (var val in list)
                {
                    Console.WriteLine(" - " + val);
                }
                Console.WriteLine();
            }
        }

        private void OutputDiff(Diff diff)
        {
            foreach (TargetDiff targetDiff in diff.Targets)
            {
                OutputHeader(1, targetDiff.Name);
                OutputTargetDiff(targetDiff);
            }
        }

        private void OutputTargetDiff(TargetDiff diff)
        {
            foreach(string key in diff.Diffs.Keys)
            {
                OutputHeader(2, key);
                OutputStringsDiff(diff.Diffs[key]);
            }
        }
        private void OutputStringsDiff(StringsDiff diff)
        {
            OutputDiffList("Left", diff.Left);
            OutputDiffList("Both", diff.Both);
            OutputDiffList("Right", diff.Right);
        }

        private void OutputDiffList(string name, List<string> list)
        {
            if((list == null) || (list.Count == 0))
            {
                return;
            }

            OutputHeader(3, name);
            OutputList(list);
        }
    }
}
