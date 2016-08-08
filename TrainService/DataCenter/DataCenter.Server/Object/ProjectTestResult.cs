using ContinuousIntegration.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCenter.Server.Object
{
    class ProjectTestResult
    {
        public TestCase TestCase { get; private set; }
        public AssignTo Assignment { get; private set; }
        public TestCaseResult EarliestResult { get; private set; }
        public TestCaseResult LatestResult { get; private set; }
        public TestCaseResult FirstBrokenResult { get; private set; }
        public bool IsFirstBrokenTrusted { get; private set; }


        private SortedList<int, TestCaseResult> history = new SortedList<int, TestCaseResult>();

        public ProjectTestResult(TestCase testcase, AssignTo assignTo)
        {
            TestCase = testcase;
            Assignment = assignTo;
            EarliestResult = null;
            FirstBrokenResult = null;
            IsFirstBrokenTrusted = false;
        }

        public void AddResults(TestCaseResult[] results)
        {
            if (results != null && results.Count() > 0)
            {
                foreach (var r in results)
                {
                    if (!history.ContainsKey(r.ChangeSet) && r.ChangeSet > 0)
                    {
                        history.Add(r.ChangeSet, r);
                    }
                }
            }

            FirstBrokenResult = null;
            IsFirstBrokenTrusted = true;

            if (history.Count > 0)
            {
                EarliestResult = history.Values.First();
                LatestResult = history.Values.Last();
            }

            if (LatestResult != null && LatestResult.Passed)
            {
                FirstBrokenResult = null;
                IsFirstBrokenTrusted = true;
            }

            if (LatestResult != null && !LatestResult.Passed && history.Count > 1)
            {
                for (int i = history.Count - 1; i > 0; i--)
                {
                    if (history.Values[i - 1].Passed)
                    {
                        FirstBrokenResult = history.Values[i];
                        IsFirstBrokenTrusted = false;
                        break;
                    }
                }
            }            
        }

        public void UpdateConfidence(SortedList<int, string> queue, Dictionary<string, TestRun> runs)
        {
            if (FirstBrokenResult != null)
            {
                IsFirstBrokenTrusted = ComputeConfidence(FirstBrokenResult, queue, runs);
            }
            else
            {
                IsFirstBrokenTrusted = true;                
            }
        }

        private static bool ComputeConfidence(TestCaseResult firstBrokenResult, SortedList<int, string> queue, Dictionary<string, TestRun> runs)
        {
            var prevArray = queue.Keys.Where(k => k < firstBrokenResult.ChangeSet).ToArray();
            if (prevArray.Length > 0)
            {
                var prev = prevArray.Max();
                if (!queue.ContainsKey(prev))
                {
                    return false;
                }
                else
                {
                    var runId = queue[prev];
                    if (string.IsNullOrEmpty(runId) || !runs.ContainsKey(runId))
                    {
                        return false;
                    }
                    else
                    {
                        return runs[runId].CompletedTime > runs[runId].StartedTime;
                    }
                }
            }
            else
            {
                return true;
            }
        }

        public void UpdateAssignment(AssignTo assign)
        {
            if (assign.FullName == TestCase.FullName)
            {
                Assignment = assign;
            }
        }

        public int Count
        {
            get
            {
                return history.Count;
            }
        }

        public bool Passed
        {
            get
            {
                if (LatestResult != null)
                {
                    return LatestResult.Passed;
                }

                return false;
            }
        }

        public bool Failed
        {
            get
            {
                if (LatestResult != null)
                {
                    return !LatestResult.Passed;
                }

                return false;
            }
        }

        /// <summary>
        /// Testcase result history. Inverted, latest is first.
        /// </summary>
        public TestCaseResult[] History
        {
            get
            {
                if (history.Count > 0)
                {
                    return history.Values.Reverse().ToArray();
                }
                else
                {
                    return new TestCaseResult[] { };
                }
            }
        }

        /// <summary>
        /// Recent result history. Sorted, earliest is first.
        /// </summary>
        public TestCaseResult[] RecentHistory
        {
            get
            {
                if (history.Count > 50)
                {
                    return history.Values.Skip(history.Count - 50).ToArray();
                }
                else if (history.Count > 0)
                {
                    return history.Values.ToArray();
                }
                else
                {
                    return new TestCaseResult[] { };
                }
            }
        }
    }
}
