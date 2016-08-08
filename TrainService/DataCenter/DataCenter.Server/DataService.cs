using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Javis.Mark42.Interface;
using DataCenter.DataAccess;
using DataCenter.Server.Object;
using ContinuousIntegration.Interface;
using Nebula.Redis;
using Nebula.Log;
using Nebula.MongoDB;

namespace DataCenter.Server
{
    class DataService : TaskBasedServiceBase
    {
        #region Initialization and members

        public static DataService Instance { get; private set; }

        static DataService()
        {
            Instance = new DataService();
        }

        private DataLogic global;

        private DataService()            
        {
            base.Interval = new TimeSpan(0, 3, 0);
            global = new DataLogic();

            Logger.Log("DataCenter.Server.Initialize", true)
                    .SubmitAsInfo();
        }

        #endregion

        #region Getters

        public IDictionary<TestRun, TestRunStatus> GetTestRunsByCoreid(string coreid)
        {
            return global.GetTestRunsByCoreid(coreid);
        }

        public IDictionary<TestRun, TestRunStatus> GetTestRuns(int page, string projectType)
        {
            if (page <= 0)
            {
                page = 1;
            }

            return global.GetTestRuns(page, projectType);
        }

        public KeyValuePair<TestRun, TestRunStatus> GetTestRun(string runId)
        {
            return global.GetTestRun(runId);
        }

        public TestRunResult GetTestRunResult(string runId)
        {
            return global.GetTestRunResult(runId);
        }

        public ProjectTestResult GetTestCaseByName(string fullname, string projectType)
        {
            return global.GetTestCase(fullname, projectType);
        }

        public ProjectTestResult[] GetFailedTestCases(string projectType)
        {
            return global.GetFailedTestCases(projectType);
        }

        #endregion

        #region Setters

        public bool Assign(AssignTo to, string projectType)
        {
            if (to != null && !string.IsNullOrEmpty(projectType) && !string.IsNullOrEmpty(to.FullName) && !string.IsNullOrEmpty(to.AssignToEngineer))
            {
                return global.Assign(to, projectType);
            }

            return false;
        }
        #endregion

        protected override void DoWork()
        {
            try
            {
                GC.Collect();
                global.LoadData();
            }
            catch(Exception ex)
            {
                var u = new ExceptionEvent("DataCenter.DataService.Loop", ex);
                Logger.LogException(u);
            }
        }

        #region Helpers

        #endregion
    }
}
