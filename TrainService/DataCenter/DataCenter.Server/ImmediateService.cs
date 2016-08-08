using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nebula.Log;
using Nebula.Redis;
using ContinuousIntegration.Interface;
using ContinuousIntegration.DataAccess;

namespace DataCenter.Server
{
    class ImmediateService
    {
        public static ImmediateService Instance { get; private set; }

        static ImmediateService()
        {
            Instance = new ImmediateService();
        }

        private RedisConnection connRedis;
        private Repository Repository;

        private ImmediateService()
        {
            string ciServer = ConfigurationManager.AppSettings.Get("ContinuousIntegration");
            this.connRedis = new RedisConnection(ciServer);

            if (this.connRedis.IsConnected)
            {
                this.Repository = new Repository(this.connRedis.Accessor);
            }

            if (this.Repository != null)
            {
                Logger.Log("DataCenter.Server.ImmediateService.Connect", true)
                    .Append("Server", ciServer)
                    .SubmitAsInfo();
            }
            else
            {
                Logger.Log("DataCenter.Server.ImmediateService.Connect", false)
                    .Append("Status", this.Repository != null ? "Connected" : "Disconnected")
                    .Append("Server", ciServer)
                    .Append("Expection", this.connRedis.ConnectionException != null ? this.connRedis.ConnectionException.Message : "")
                    .SubmitAsError();
            }
        }

        public IDictionary<TestRun, TestRunStatus> GetActiveTestRuns()
        {
            IDictionary<TestRun, TestRunStatus> runs = new Dictionary<TestRun, TestRunStatus>();

            var ids = this.Repository.GetAllTestRunsIds();
            foreach(var id in ids)
            {
                var run = this.Repository.GetTestRun(id);
                if (run != null)
                {
                    var status = this.Repository.GetTestRunStatus(id);
                    if (status == null)
                    {
                        status = new TestRunStatus() { Identifier = id, MainWorkFlowId = Guid.Empty.ToString() };
                    }
                    runs.Add(run, status);
                }
            }

            return runs;
        }

        public KeyValuePair<TestRun, TestRunStatus> GetTestRun(string identifier)
        {
            var r = this.Repository.GetTestRun(identifier);
            var s = this.Repository.GetTestRunStatus(identifier);

            if (r != null)
            {
                if (s == null)
                {
                    s = new TestRunStatus() { Identifier = r.Identifier, MainWorkFlowId = Guid.Empty.ToString() };
                }

                return new KeyValuePair<TestRun, TestRunStatus>(r, s);
            }

            return new KeyValuePair<TestRun, TestRunStatus>();
        }

        public TestRunResult GetResult(string identifier)
        {
            return this.Repository.GetTestRunResult(identifier);
        }
    }
}
