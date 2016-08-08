using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using CI = ContinuousIntegration.Interface;

namespace DataCenter.DataAccess
{
    public class Repository
    {
        private const int PAGE_SIZE = 100;

        protected IMongoClient client;

        protected IMongoDatabase dbTestCases;
        protected IMongoDatabase dbTestRuns;
        protected IMongoDatabase dbTestResults;

        protected IMongoCollection<BsonDocument> colTestCases;
        protected IMongoCollection<BsonDocument> colInactiveFullnames;
        protected IMongoCollection<BsonDocument> colAssignments;
        protected IMongoCollection<BsonDocument> colTestRuns;
        protected IMongoCollection<BsonDocument> colTestResults;

        private TestCaseRepository TestCases;

        public Repository(string connectionString)
            : this(new MongoClient(connectionString))
        { }

        public Repository(IMongoClient existClient)
        {
            this.client = existClient;

            this.dbTestCases = this.client.GetDatabase("TestCases");
            this.dbTestRuns = this.client.GetDatabase("TestRuns");
            this.dbTestResults = this.client.GetDatabase("TestResults");

            this.colTestCases = this.dbTestCases.GetCollection<BsonDocument>("ActiveTestCases");
            this.colInactiveFullnames = this.dbTestCases.GetCollection<BsonDocument>("InactiveFullnames");
            this.colAssignments = this.dbTestCases.GetCollection<BsonDocument>("ActiveAssignments");
            this.colTestRuns = this.dbTestRuns.GetCollection<BsonDocument>("ActiveTestRuns");
            this.colTestResults = this.dbTestResults.GetCollection<BsonDocument>("ActiveTestResults");

            this.TestCases = new TestCaseRepository(this.colTestCases);
        }

        #region Create Indexes
        //private void AddIndexes()
        //{
        //    var wait = this.colTestCases.Indexes.CreateOneAsync(string.Format("{{ \"{0}\" : 1 }}", Constants.TestCase_Identifier), new CreateIndexOptions() { Unique = true });
        //    wait.Wait();

        //    wait = this.colTestCases.Indexes.CreateOneAsync(string.Format("{{ \"{0}\" : 1 }}", Constants.TestCase_Fullname), new CreateIndexOptions() { Unique = true });
        //    wait.Wait();

        //    wait = this.colTestCases.Indexes.CreateOneAsync(string.Format("{{ \"{0}\" : 1 }}", Constants.TestCase_Fullname), new CreateIndexOptions() { Unique = true });
        //    wait.Wait();

        //    wait = this.colTestResults.Indexes.CreateOneAsync(string.Format("{{ \"{0}\" : 1, \"{1}\" : 1 }}", Constants.TestResult_TestRun, Constants.TestResult_TestCase), new CreateIndexOptions() { Unique = true });
        //    wait.Wait();

        //    wait = this.colTestResults.Indexes.CreateOneAsync(string.Format("{{ \"{0}\" : 1, \"{1}\" : -1 }}", Constants.TestResult_TestCase, Constants.TestResult_ChangeSet));
        //    wait.Wait();

        //    wait = this.colTestResults.Indexes.CreateOneAsync(string.Format("{{ \"{0}\" : 1 }}", Constants.TestResult_TestRun));
        //    wait.Wait();

        //    wait = this.colTestRuns.Indexes.CreateOneAsync(string.Format("{{ \"{0}\" : 1 }}", Constants.TestRun_Identifier), new CreateIndexOptions() { Unique = true });
        //    wait.Wait();
        //}
        #endregion

        #region TestCase

        public void ReloadTestCases()
        {
            this.TestCases.Reload();
        }

        public string GetTestCaseIdentifierByFullname(string fullname)
        {
            var testcase = this.TestCases.GetByName(fullname);
            if (testcase != null)
            {
                return testcase.Identifier;
            }

            return string.Empty;
        }

        public string[] GetInactiveFullnames()
        {
            var wait = this.colInactiveFullnames.Find(new BsonDocument { }).ToListAsync();
            wait.Wait();

            if (wait.Result != null && wait.Result.Count > 0)
            {
                return wait.Result.Select(n => n.GetValue(Constants.TestCase_Fullname, string.Empty).AsString).ToArray();
            }

            return new string[] { };
        }

        public void SetInactiveFullname(string fullname)
        {
            var document = new BsonDocument
            {
                { Constants.TestCase_Fullname, fullname }
            };

            var wait = this.colInactiveFullnames.InsertOneAsync(document);
            wait.Wait();
        }

        public CI.TestCase[] GetAllTestCases()
        {
            return this.TestCases.GetAll();
        }

        public bool Assign(string identifier, CI.AssignTo assign)
        {
            var last = this.GetAssignment(identifier, assign.Project);

            if (last != null)
            {
                if (last.AssignToEngineer == assign.AssignToEngineer
                    && last.AssignToChangeset == assign.AssignToChangeset
                    && last.Project == assign.Project)
                {
                    // Same info.
                    return false;
                }
            }

            var document = new BsonDocument
            {
                { Constants.Assignment_TestCase, identifier },
                { Constants.Assignment_Engineer, assign.AssignToEngineer },
                { Constants.Assignment_ChangeSet, assign.AssignToChangeset },
                { Constants.Assignment_ProjectType, assign.Project },
                { Constants.Assignment_Comment, assign.Comment },
                { Constants.Assignment_Time, assign.AssignToTime.ToUniversalTime() },
            };

            var wait = this.colAssignments.InsertOneAsync(document);
            wait.Wait();
            return true;
        }

        public CI.AssignTo GetAssignment(string testcaseId, string projectType)
        {
            var filter = Builders<BsonDocument>.Filter.Eq(Constants.Assignment_TestCase, testcaseId)
                         & Builders<BsonDocument>.Filter.Eq(Constants.Assignment_ProjectType, projectType);
            var sort = Builders<BsonDocument>.Sort.Descending(Constants.Assignment_Time);

            var wait = this.colAssignments.Find(filter).Sort(sort).Limit(1).FirstOrDefaultAsync();
            wait.Wait();

            if (wait.Result != null)
            {
                return wait.Result.ToAssignment();
            }

            return null;
        }

        #endregion

        #region TestRun

        public KeyValuePair<CI.TestRun, CI.TestRunStatus> GetTestRun(string identifier)
        {
            var filter = Builders<BsonDocument>.Filter.Eq(Constants.TestRun_Identifier, identifier);

            var wait = this.colTestRuns.Find(filter).FirstOrDefaultAsync();
            wait.Wait();

            if (wait.Result != null)
            {
                return new KeyValuePair<CI.TestRun, CI.TestRunStatus>(wait.Result.ToTestRun(), wait.Result.ToTestStatus());
            }

            return new KeyValuePair<CI.TestRun,CI.TestRunStatus>();
        }

        public IDictionary<CI.TestRun, CI.TestRunStatus> GetAllTestRuns()
        {
            var runs = new Dictionary<CI.TestRun, CI.TestRunStatus>();

            var sort = Builders<BsonDocument>.Sort.Descending(Constants.TestRun_StartedTime);

            var wait = this.colTestRuns.Find(new BsonDocument { }).Sort(sort).ToListAsync();
            wait.Wait();

            if (wait.Result != null)
            {
                foreach(var d in wait.Result)
                {
                    var run = d.ToTestRun();
                    var status = d.ToTestStatus();
                    runs.Add(run, status);
                }
            }

            return runs;
        }

        public IDictionary<CI.TestRun, CI.TestRunStatus> GetTestRuns(DateTime time)
        {
            var runs = new Dictionary<CI.TestRun, CI.TestRunStatus>();

            var filter = Builders<BsonDocument>.Filter.Lte(Constants.TestRun_StartedTime, time.ToUniversalTime());
            var sort = Builders<BsonDocument>.Sort.Descending(Constants.TestRun_StartedTime);

            var wait = this.colTestRuns.Find(filter).Sort(sort).Limit(PAGE_SIZE).ToListAsync();
            wait.Wait();

            if (wait.Result != null)
            {
                foreach (var d in wait.Result)
                {
                    var run = d.ToTestRun();
                    var status = d.ToTestStatus();
                    runs.Add(run, status);
                }
            }

            return runs;
        }

        public void ImportTestRunWithResults(CI.TestRun run, CI.TestRunStatus workflows, CI.TestRunResult result)
        {
            if (run != null)
            {
                this.SaveTestRun(run, workflows);
                this.SaveTestResults(run, result);
            }
        }

        public void UpdateTestRun(CI.TestRun run, CI.TestRunStatus workflows)
        {
            if (run != null)
            {
                this.SaveTestRun(run, workflows);
            }
        }

        #endregion

        #region Result

        //public void UpdateResultChangeSet(string testcaseId, string testRunId, int changeSet)
        //{
        //    var filterBuilder = Builders<BsonDocument>.Filter;
        //    var filter = filterBuilder.Eq(Constants.TestResult_TestRun, testRunId) &
        //                 filterBuilder.Eq(Constants.TestResult_TestCase, testcaseId);

        //    var update = Builders<BsonDocument>.Update
        //        .Set(Constants.TestResult_ChangeSet, changeSet);

        //    var wait = this.colTestResults.UpdateOneAsync(filter, update);
        //    wait.Wait();
        //}

        public CI.TestCaseResult[] GetTestCaseResults(CI.TestCase testcase, string projectType,  int limit)
        {
            var filter = Builders<BsonDocument>.Filter.Eq(Constants.TestResult_TestCase, testcase.Identifier) & 
                 Builders<BsonDocument>.Filter.Eq(Constants.TestResult_TestRunType, projectType);
            var sort = Builders<BsonDocument>.Sort.Descending(Constants.TestResult_ChangeSet);

            var wait = this.colTestResults.Find(filter).Sort(sort).Limit(limit).ToListAsync();
            wait.Wait();

            if (wait.Result != null)
            {
                return wait.Result.Select(c => c.ToTestCaseResult(testcase)).ToArray();
            }

            return new CI.TestCaseResult[] { };
        }

        public CI.TestCaseResult[] GetTestCaseResults(CI.TestCase testcase, string projectType, int version, int limit)
        {
            var filter = Builders<BsonDocument>.Filter.Eq(Constants.TestResult_TestCase, testcase.Identifier) &
                 Builders<BsonDocument>.Filter.Eq(Constants.TestResult_TestRunType, projectType) &
                 Builders<BsonDocument>.Filter.Lt(Constants.TestResult_ChangeSet, version);
            var sort = Builders<BsonDocument>.Sort.Descending(Constants.TestResult_ChangeSet);

            var wait = this.colTestResults.Find(filter).Sort(sort).Limit(limit).ToListAsync();
            wait.Wait();

            if (wait.Result != null)
            {
                return wait.Result.Select(c => c.ToTestCaseResult(testcase)).ToArray();
            }

            return new CI.TestCaseResult[] { };
        }

        public long GetTestRunResultsCount(string identifier)
        {
            var filter = Builders<BsonDocument>.Filter.Eq(Constants.TestResult_TestRun, identifier);

            var wait = this.colTestResults.Find(filter).CountAsync();
            wait.Wait();

            return wait.Result;
        }

        public CI.TestRunResult GetTestRunResults(string identifier)
        {
            var result = new CI.TestRunResult() { TestRun = identifier };

            var filter = Builders<BsonDocument>.Filter.Eq(Constants.TestResult_TestRun, identifier);
            var wait = this.colTestResults.Find(filter).ToListAsync();
            wait.Wait();

            if (wait.Result != null)
            {
                foreach(var d in wait.Result)
                {
                    try
                    {
                        var r = Parse(d);
                        if (r != null)
                        {
                            result.Results.Add(r);
                        }
                    }
                    catch { }
                }
            }

            return result;
        }

        #endregion

        #region Helpers

        protected void SaveTestResults(CI.TestRun run, CI.TestRunResult result)
        {
            if (run != null && result!= null && result.Results != null && result.Results.Count > 0)
            {
                foreach(var r in result.Results)
                {
                    var addedTestCase = this.TestCases.AddOrUpdateTestCase(r.TestCase);
                    r.TestCase.Identifier = addedTestCase.Identifier;
                    this.SaveResult(r, run);
                }
            }
        }

        /// <summary>
        /// Save Test Result to test case result collection.
        /// </summary>
        protected void SaveResult(CI.TestCaseResult result, CI.TestRun run)
        {
            if (result != null)
            {
                var filterBuilder = Builders<BsonDocument>.Filter;
                var filter = filterBuilder.Eq(Constants.TestResult_TestRun, result.TestRun) & 
                             filterBuilder.Eq(Constants.TestResult_TestCase, result.TestCase.Identifier);

                var update = Builders<BsonDocument>.Update
                    .Set(Constants.TestResult_Fullname, result.TestCase.FullName)
                    .Set(Constants.TestResult_TestRunType, run.Type)
                    .Set(Constants.TestResult_ChangeSet, result.ChangeSet)
                    .Set(Constants.TestResult_MachineName, result.TestMachineName)
                    .Set(Constants.TestResult_Passed, result.Passed)
                    .Set(Constants.TestResult_Duration, result.Duration.Ticks)
                    .Set(Constants.TestResult_Message, result.Message);

                var wait = this.colTestResults.UpdateOneAsync(filter, update, new UpdateOptions() { IsUpsert = true });
                wait.Wait();
            }
        }

        protected void SaveTestRun(CI.TestRun run, CI.TestRunStatus workflows)
        {
            if (run != null)
            {
                var filter = Builders<BsonDocument>.Filter.Eq(Constants.TestRun_Identifier, run.Identifier);
                var update = Builders<BsonDocument>.Update
                    .Set(Constants.TestRun_Owner, run.Owner)
                    .Set(Constants.TestRun_Build, run.Build)
                    .Set(Constants.TestRun_Type, run.Type)
                    .Set(Constants.TestRun_Flies, run.Flies)
                    .Set(Constants.TestRun_DropSourceLocation, run.DropSourceLocation)
                    .Set(Constants.TestRun_StartedTime, run.StartedTime)
                    .Set(Constants.TestRun_CompletedTime, run.CompletedTime);

                if (run is CI.TestRunWithBuildUri)
                {
                    update = update.Set(Constants.TestRun_BuildUri, (run as CI.TestRunWithBuildUri).BuildUri)
                                   .Set(Constants.TestRun_BaselineVersion, (run as CI.TestRunWithBuildUri).BaselineVersion);
                }
                if (run is CI.TestRunWithChangeSet)
                {
                    update = update.Set(Constants.TestRun_ChangeSet, (run as CI.TestRunWithChangeSet).ChangeSet);
                    if ((run as CI.TestRunWithChangeSet).IsFullTestRun)
                    {
                        update = update.Set(Constants.TestRun_IsFullTestRun, true);
                    }
                }

                if (workflows != null)
                {
                    update = update.Set(Constants.TestRun_MainWorkflow, workflows.MainWorkFlowId);

                    List<BsonDocument> allWorkflows = new List<BsonDocument>();
                    foreach(var w in workflows.CompletedTasks)
                    {
                        allWorkflows.Add(new BsonDocument 
                        {
                            {Constants.TestRun_Workflow_Identifier, w.Key},
                            {Constants.TestRun_Workflow_Status, "Completed"},
                            {Constants.TestRun_Workflow_Name, w.Value}
                        });
                    }
                    foreach (var w in workflows.AbortedTasks)
                    {
                        allWorkflows.Add(new BsonDocument 
                        {
                            {Constants.TestRun_Workflow_Identifier, w.Key},
                            {Constants.TestRun_Workflow_Status, "Aborted"},
                            {Constants.TestRun_Workflow_Name, w.Value}
                        });
                    }

                    update.Set(Constants.TestRun_Workflows, allWorkflows.ToArray());
                }

                var wait = this.colTestRuns.UpdateOneAsync(filter, update, new UpdateOptions() { IsUpsert = true });
                wait.Wait();
            }
        }

        private CI.TestCaseResult Parse(BsonDocument data)
        {
            var tcId = data[Constants.TestResult_TestCase].AsString;
            var testcase = this.TestCases.GetById(tcId);

            if (testcase != null)
            {
                return data.ToTestCaseResult(testcase);
            }

            return null;
        }
        
        #endregion

    }
}
