using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DataCenter.Server.Controller
{
    [EnableCors("*", "*", "GET")]
    [RoutePrefix("api")]
    public class TestRunsController : ApiController
    {
        [Route("TestRuns")]
        public IEnumerable<object> Get()
        {
            return GetTestRuns("PostCheckin", 1);
        }

        [Route("TestRuns/{projectType}")]
        public IEnumerable<object> Get(string projectType)
        {
            return GetTestRuns(projectType, 1);
        }

        [HttpGet, Route("TestRuns/{projectType}/{page:int:min(1)}")]
        public object GetTestRunsPerPage(string projectType, int page)
        {
            return GetTestRuns(projectType, page);
        }

        [HttpGet, Route("TestRuns/{projectType}/{coreid:length(6)}")]
        public object GetTestRunByCoreID(string projectType, string coreid)
        {
            return GetTestRunsByCoreid(coreid.ToUpper());
        }

        [Route("TestRuns/{projectType}/{runId}")]
        public object Get(string projectType, string runId)
        {
            return GetTestRun(runId);
        }

        private object GetTestRun(string id)
        {
            var data = DataService.Instance.GetTestRun(id);
            if (data.Key != null)
            {
                return GetObject(data);
            }

            return new { Id = id, State = "Unknown" };
        }

        private IEnumerable<object> GetTestRuns(string projectType, int page)
        {
            var data = DataService.Instance.GetTestRuns(page, projectType);
            var runs = data.Select(d => GetObjectWithoutResult(d));

            return runs;
        }

        private IEnumerable<object> GetTestRunsByCoreid(string coreid)
        {
            var data = DataService.Instance.GetTestRunsByCoreid(coreid);
            var runs = data.Select(d => GetObjectWithoutResult(d));

            return runs;
        }

        private object GetObject(KeyValuePair<ContinuousIntegration.Interface.TestRun, ContinuousIntegration.Interface.TestRunStatus> data)
        {
            var result = DataService.Instance.GetTestRunResult(data.Key.Identifier);
            return new
            {
                Identifier = data.Key.Identifier,
                State = data.Key.CompletedTime >= data.Key.StartedTime ? "Completed" : "Running",
                Type = data.Key.Type,
                Build = data.Key.Build,
                Owner = NameService.GetCoreId(data.Key.Owner),
                OwnerName = NameService.Instance.GetUserName(data.Key.Owner),
                StartedTime = data.Key.StartedTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"),
                CompletedTime = data.Key.CompletedTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"),
                WorkFlow = data.Value.MainWorkFlowId,
                Changes = data.Key.Flies,
                ChangeSet = data.Key is ContinuousIntegration.Interface.TestRunWithChangeSet ? (data.Key as ContinuousIntegration.Interface.TestRunWithChangeSet).ChangeSet : 0,
                Results = result.Results.Select(r => new
                {
                    Passed = r.Passed,
                    FullName = r.TestCase.FullName,
                    Owner = NameService.GetCoreId(r.TestCase.Owner),
                    OwnerName = NameService.Instance.GetUserName(r.TestCase.Owner),
                    Scope = r.TestCase.Scope,
                    Machine = r.TestMachineName,
                    Msg = Utility.FormatMessage(r.Message),
                    Duration = r.Duration.TotalMilliseconds
                })
            };
        }

        private object GetObjectWithoutResult(KeyValuePair<ContinuousIntegration.Interface.TestRun, ContinuousIntegration.Interface.TestRunStatus> data)
        {
            return new
            {
                Identifier = data.Key.Identifier,
                State = data.Key.CompletedTime >= data.Key.StartedTime ? "Completed" : "Running",
                Type = data.Key.Type,
                Build = data.Key.Build,
                Owner = NameService.GetCoreId(data.Key.Owner),
                OwnerName = NameService.Instance.GetUserName(data.Key.Owner),
                StartedTime = data.Key.StartedTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"),
                CompletedTime = data.Key.CompletedTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"),
                WorkFlow = data.Value.MainWorkFlowId,
                Changes = data.Key.Flies,
                ChangeSet = data.Key is ContinuousIntegration.Interface.TestRunWithChangeSet ? (data.Key as ContinuousIntegration.Interface.TestRunWithChangeSet).ChangeSet : 0
            };
        }
    }
}
