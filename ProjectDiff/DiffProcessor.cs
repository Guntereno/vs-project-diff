using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ProjectDiff
{
    public class DiffProcessor
    {
        public Diff Result
        {
            get;
            private set;
        }

        private Config _config;

        public DiffProcessor(Config config)
        {
            _config = config;

            var projects = new ProjectFileCache();

            var result = new Diff();

            result.Targets = new List<TargetDiff>();

            foreach (var mapping in config.Mappings)
            {
                ProjectFileModel left = projects[mapping.Left.ProjectPath];
                TargetModel leftTarget = left.FindTarget(mapping.Left.Config, mapping.Left.Platform);

                ProjectFileModel right = projects[mapping.Right.ProjectPath];
                TargetModel rightTarget = right.FindTarget(mapping.Right.Config, mapping.Right.Platform);

                var diff = CreateDiff(leftTarget, rightTarget);
                diff.Name = mapping.Name;
                result.Targets.Add(diff);
            }

            Result = result;
        }

        private TargetDiff CreateDiff(TargetModel left, TargetModel right)
        {
            var diff = new TargetDiff();

            Type targetModelType = typeof(TargetModel);
            MemberInfo[] members = targetModelType.GetMembers(BindingFlags.Public | BindingFlags.Instance);

            diff.Diffs = new Dictionary<string, StringsDiff>();

            // Create a diff for each list of strings in the type
            Type listStringT = typeof(List<string>);
            foreach(var memberInfo in members)
            {
                if((memberInfo.MemberType == MemberTypes.Field) &&
                    memberInfo.GetUnderlyingType() == listStringT)
                {
                    var leftStrs = (List<string>)((FieldInfo)memberInfo).GetValue(left);
                    var rightStrs = (List<string>)((FieldInfo)memberInfo).GetValue(right);

                    diff.Diffs[memberInfo.Name] = CreateDiff(leftStrs, rightStrs);
                }
            }

            return diff;
        }

        private StringsDiff CreateDiff(List<string> left, List<string> right, bool ignoreCase = false)
        {
            StringComparer cmp = ignoreCase ?
                    StringComparer.OrdinalIgnoreCase :
                    StringComparer.Ordinal;
            var leftSet = new HashSet<string>(left, cmp);
            var rightSet = new HashSet<string>(right, cmp);

            var diff = new StringsDiff();

            if((_config.Ignore & IgnoreFlags.Left) == 0)
            {
                diff.Left = leftSet.Except(rightSet).ToList();
                diff.Left.Sort();
            }

            if ((_config.Ignore & IgnoreFlags.Both) == 0)
            {
                diff.Both = leftSet.Intersect(rightSet).ToList();
                diff.Both.Sort();
            }

            if ((_config.Ignore & IgnoreFlags.Right) == 0)
            {
                diff.Right = rightSet.Except(leftSet).ToList();
                diff.Right.Sort();
            }

            return diff;
        }
    }
}
