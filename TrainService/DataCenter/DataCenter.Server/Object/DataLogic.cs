using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Javis.Mark42.Interface;
using DataCenter.Server.Object;
using ContinuousIntegration.Interface;
using Nebula.Redis;
using Nebula.Log;
using Nebula.MongoDB;
using CIDA = ContinuousIntegration.DataAccess;
using DCDA = DataCenter.DataAccess;

namespace DataCenter.Server.Object
{
    class DataLogic
    {
        #region Initialization       

        private MongoConnection connMongo;
        private RedisConnection connRedis;
        
        protected DCDA.Repository Repository { get; private set; }

        public DataLogic()
        {
            string ciServer = ConfigurationManager.AppSettings.Get("ContinuousIntegration");
            this.connRedis = new RedisConnection(ciServer);

            string dataServer = ConfigurationManager.AppSettings.Get("DataCenter");
            this.connMongo = new MongoConnection(dataServer);

            if (this.connMongo.IsConnected)
            {
                this.Repository = new DCDA.Repository(this.connMongo.Client);
            }

            if (this.Repository != null)
            {
                Logger.Log("DataCenter.DataService.Connect", true)
                    .Append("Server", dataServer)
                    .SubmitAsInfo();
            }
            else
            {
                Logger.Log("DataCenter.DataService.Connect", false)
                    .Append("Status", this.Repository != null ? "Connected" : "Disconnected")
                    .Append("Server", dataServer)
                    .SubmitAsError();
            }
        }

        #endregion

        #region Members
        private object sync = new object();
        private System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

        private Dictionary<string, Project> Projects = new Dictionary<string, Project>();
        private Dictionary<string, Owner> Owners = new Dictionary<string, Owner>();

        private Dictionary<string, string> Mapping = new Dictionary<string, string>();
        #endregion

        public void LoadData()
        {
            try
            {              
                var projects = new Dictionary<string, Project>();
                var owners = new Dictionary<string, Owner>();
                var mapping = new Dictionary<string, string>();

                #region Load Test Runs
                stopwatch.Reset();
                stopwatch.Start();

                var dictRuns = this.Repository.GetAllTestRuns();
                foreach (var p in dictRuns)
                {
                    try
                    {
                        var coreid = p.Key.Owner.ToUpper();
                        if (!owners.ContainsKey(coreid))
                        {
                            owners.Add(coreid, new Owner(coreid));
                        }
                        owners[coreid].AddRun(p.Key, p.Value);

                        mapping[p.Key.Identifier] = p.Key.Type;

                        if (p.Key is TestRunWithChangeSet)
                        {
                            if (!projects.ContainsKey(p.Key.Type))
                            {
                                var queue = new CIDA.InRedisTestRunQueue(p.Key.Type, this.connRedis.Accessor).GetContent();
                                if (queue.Count <= 0)
                                {
                                    queue = new CIDA.InRedisTestRunQueue(p.Key.Type, this.connRedis.Accessor).GetContent();
                                }

                                projects[p.Key.Type] = new ProjectWithChangeSet(Repository, p.Key.Type, queue);
                            }
                        }
                        else
                        {
                            if (!projects.ContainsKey(p.Key.Type))
                            {
                                projects[p.Key.Type] = new Project(p.Key.Type);
                            }
                        }
                        projects[p.Key.Type].AddTestRun(p.Key, p.Value);
                    }
                    catch (Exception ex)
                    {
                        var u = new ExceptionEvent("DataCenter.DataService.LoadTestRun", ex);
                        if (p.Key != null)
                        {
                            u.Fields.Add("TestRun", p.Key.Identifier);
                        }                        
                        Logger.LogException(u);
                    }
                }

                var countOfRuns = projects.Sum(p => p.Value.Runs.Count);

                stopwatch.Stop();
                Logger.Log()
                    .Append("Event", "DataCenter.DataService.LoadTestRuns")
                    .Append("Elapsed_ms", stopwatch.ElapsedMilliseconds.ToString())
                    .SubmitAsDebug();

                Logger.Log("DataCenter.DataService.LoadData", true)
                    .Append("Projects", projects.Count.ToString())
                    .Append("TestRuns", countOfRuns.ToString())
                    .SubmitAsInfo();
                #endregion

                #region Load Test Cases
                stopwatch.Reset();
                stopwatch.Start();

                var list = new List<ProjectWithChangeSet>();

                foreach(var pair in projects)
                {
                    if (pair.Value is ProjectWithChangeSet)
                    {
                        list.Add(pair.Value as ProjectWithChangeSet);                        
                    }
                }

                foreach(var project in list)
                {
                    try
                    {
                        project.LoadTestCases();
                    }
                    catch (Exception ex)
                    {
                        var u = new ExceptionEvent("DataCenter.DataService.LoadProjectsTestCases", ex);
                        if (project != null)
                        {
                            u.Fields.Add("Project", project.Type);
                        }
                        Logger.LogException(u);
                    }
                }

                stopwatch.Stop();
                Logger.Log()
                    .Append("Event", "DataCenter.DataService.LoadTestCases")
                    .Append("Elapsed_ms", stopwatch.ElapsedMilliseconds.ToString())
                    .SubmitAsDebug();
                #endregion

                lock (sync)
                {
                    this.Projects = projects;
                    this.Owners = owners;
                    this.Mapping = mapping;
                }
            }
            catch (Exception ex)
            {
                var u = new ExceptionEvent("DataCenter.DataService.LoadData", ex);
                Logger.LogException(u);
            }
        }

        public ProjectTestResult[] GetFailedTestCases(string projectType)
        {
            lock (sync)
            {
                if (Projects.ContainsKey(projectType))
                {
                    var proj = Projects[projectType] as ProjectWithChangeSet;
                    if (proj != null)
                    {
                        return proj.GetFailedTestCases();
                    }
                }
            }

            return new ProjectTestResult[] { };
        }

        public ProjectTestResult GetTestCase(string fullname, string projectType)
        {
            lock (sync)
            {
                if (Projects.ContainsKey(projectType))
                {
                    var proj = Projects[projectType] as ProjectWithChangeSet;
                    if (proj != null)
                    {
                        return proj.GetTestCase(fullname);
                    }
                }
            }

            return null;
        }

        public bool Assign(AssignTo to, string projectType)
        {
            lock (sync)
            {
                if (Projects.ContainsKey(projectType))
                {
                    var proj = Projects[projectType] as ProjectWithChangeSet;
                    if (proj != null)
                    {
                        return proj.Assign(to);
                    }
                }
            }

            return false;
        }

        public TestRunResult GetTestRunResult(string runIdentifier)
        {
            return Repository.GetTestRunResults(runIdentifier);
        }

        public KeyValuePair<TestRun, TestRunStatus> GetTestRun(string runIdentifier)
        {
            lock (sync)
            {
                if (Mapping.ContainsKey(runIdentifier))
                {
                    var type = Mapping[runIdentifier];
                    if (Projects.ContainsKey(type))
                    {
                        return Projects[type].GetTestRun(runIdentifier);
                    }
                }
            }

            return new KeyValuePair<TestRun, TestRunStatus>();
        }

        public IDictionary<TestRun, TestRunStatus> GetTestRuns(int page, string projectType)
        {
            lock (sync)
            {
                if (Projects.ContainsKey(projectType))
                {
                    return Projects[projectType].GetTestRuns(page);
                }
            }

            return null;
        }

        public IDictionary<TestRun, TestRunStatus> GetTestRunsByCoreid(string coreid)
        {
            lock (sync)
            {
                if (Owners.ContainsKey(coreid))
                {
                    return Owners[coreid].Runs;
                }
            }

            return null;
        }

    }
}
