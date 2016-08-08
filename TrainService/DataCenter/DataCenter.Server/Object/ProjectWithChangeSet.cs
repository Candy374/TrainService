using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContinuousIntegration.Interface;
using DataCenter.DataAccess;
using Nebula.Log;

namespace DataCenter.Server.Object
{
    class ProjectWithChangeSet : Project
    {
        /// <summary>
        /// The identifier of latest, completed full test run.
        /// </summary>
        public TestRunWithChangeSet Latest { get; private set; }

        private SortedList<int, string> queue;
        private Dictionary<string, ProjectTestResult> results;
        private Dictionary<string, string> mapping;
        private ProjectTestResult[] failed;

        private Repository Repository;

        public ProjectWithChangeSet(Repository repository, string type, SortedList<int, string> q)
            : base(type)        
        {
            Repository = repository;
            Latest = new TestRunWithChangeSet() { Identifier = string.Empty, ChangeSet = 0, Type = type };
            results = new Dictionary<string, ProjectTestResult>();
            mapping = new Dictionary<string, string>();
            failed = new ProjectTestResult[] { };
            queue = q;            
        }

        public ProjectTestResult[] GetFailedTestCases()
        {
            return failed;
        }

        public ProjectTestResult GetTestCase(string fullname)
        {
            if (mapping.ContainsKey(fullname))
            {
                var id = mapping[fullname];
                if (results.ContainsKey(id))
                {
                    return results[id];
                }
            }

            return null;
        }

        public bool Assign(AssignTo to)
        {
            if (mapping.ContainsKey(to.FullName))
            {
                var id = mapping[to.FullName];
                to.Project = Type;
                var success = Repository.Assign(id, to);
                
                if (success && results.ContainsKey(id))
                {
                    results[id].UpdateAssignment(to);
                }

                return success;
            }

            return false;
        }

        #region Add runs and Test cases

        public override void AddTestRun(TestRun run, TestRunStatus status)
        {
            if (run is TestRunWithChangeSet)
            {
                base.AddTestRun(run, status);

                var runWCS = run as TestRunWithChangeSet;
                if (runWCS.IsFullTestRun && runWCS.CompletedTime > runWCS.StartedTime && runWCS.ChangeSet > Latest.ChangeSet)
                {
                    UpdateLatest(runWCS);
                }
            }
        }

        private void UpdateLatest(TestRunWithChangeSet run)
        {
            Latest.Build = run.Build;
            Latest.ChangeSet = run.ChangeSet;
            Latest.CompletedTime = run.CompletedTime;
            Latest.DropSourceLocation = run.DropSourceLocation;
            Latest.Flies = run.Flies;
            Latest.Identifier = run.Identifier;
            Latest.IsFullTestRun = true;
            Latest.Owner = run.Owner;
            Latest.StartedTime = run.StartedTime;
        }

        /// <summary>
        /// Load test cases with results.
        /// </summary>
        public void LoadTestCases()
        {
            if (!string.IsNullOrEmpty(Latest.Identifier))
            {
                var runResult = Repository.GetTestRunResults(Latest.Identifier);
                if (runResult.Results.Count <= 0)
                {
                    runResult = Repository.GetTestRunResults(Latest.Identifier);
                }

                foreach (var resultInRun in runResult.Results)
                {
                    try
                    {
                        if (resultInRun.Passed)
                        {
                            AddTestCaseResult(resultInRun.TestCase, new TestCaseResult[] { resultInRun });
                        }
                        else
                        {
                            var loadedResults = Repository.GetTestCaseResults(resultInRun.TestCase, Type, 2000);
                            if (loadedResults.Length <= 0)
                            {
                                // Re-load
                                loadedResults = Repository.GetTestCaseResults(resultInRun.TestCase, Type, 2000);
                            }

                            AddTestCaseResult(resultInRun.TestCase, loadedResults);

                            // Check whether result is trusted
                            results[resultInRun.TestCase.Identifier].UpdateConfidence(queue, Runs);
                        }                        
                    }
                    catch (Exception ex)
                    {
                        var u = new ExceptionEvent("DataCenter.DataService.LoadTestCases", ex);
                        Logger.LogException(u);
                    }
                }

                if (results.Count > 0)
                {
                    failed = results.Where(p => p.Value.Failed).Select(p => p.Value).ToArray();
                }
            }
        }

        private void AddTestCaseResult(TestCase testcase, TestCaseResult[] testcaseResults)
        {
            try
            {
                if (!results.ContainsKey(testcase.Identifier))
                {
                    var assignment = Repository.GetAssignment(testcase.Identifier, Type);
                    results[testcase.Identifier] = new ProjectTestResult(testcase, assignment);                    
                }

                results[testcase.Identifier].AddResults(testcaseResults);
                mapping[testcase.FullName] = testcase.Identifier;
            }
            catch (Exception ex)
            {
                var u = new ExceptionEvent("DataCenter.DataService.LoadTestCases", ex);
                Logger.LogException(u);
            }
        }
        #endregion
    }
}
