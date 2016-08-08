using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContinuousIntegration.Interface;

namespace DataCenter.Server.Object
{
    class Project
    {
        public string Type { get; private set; }

        public Dictionary<string, TestRun> Runs { get; private set; }
        public Dictionary<string, TestRunStatus> Statuses { get; private set; }

        public Project(string type)
        {
            Type = type;
            Runs = new Dictionary<string, TestRun>();
            Statuses = new Dictionary<string, TestRunStatus>();
        }

        public virtual void AddTestRun(TestRun run, TestRunStatus status)
        {
            Runs[run.Identifier] = run;
            Statuses[run.Identifier] = status;
        }

        public virtual KeyValuePair<TestRun, TestRunStatus> GetTestRun(string runIdentifier)
        {
            TestRun run = null;
            TestRunStatus status = null;

            if (Runs.ContainsKey(runIdentifier))
            {
                run = Runs[runIdentifier];
            }

            if (Statuses.ContainsKey(runIdentifier))
            {
                status = Statuses[runIdentifier];
            }

            return new KeyValuePair<TestRun, TestRunStatus>(run, status);
        }

        public virtual IDictionary<TestRun, TestRunStatus> GetTestRuns(int page)
        {
            var runs = new Dictionary<TestRun, TestRunStatus>();

            int prev = (page - 1) * 50;
            if (Runs.Count > prev)
            {
                IList<TestRun> selected = null;

                if (prev > 0)
                {
                    selected = Runs.Skip(prev).Take(50).Select(r => r.Value).ToList();
                }
                else
                {
                    selected = Runs.Take(50).Select(r => r.Value).ToList();
                }

                if (selected != null && selected.Count > 0)
                {
                    foreach (var r in selected)
                    {
                        runs.Add(r, Statuses[r.Identifier]);
                    }
                }
            }

            return runs;
        }
    }
}
