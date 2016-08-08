using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Nebula.Log;
using Nebula.Redis;
using Nebula.MongoDB;
using Javis.Mark42.Interface;
using CI = ContinuousIntegration;
using DC = DataCenter;

namespace DataCenter.Eyes
{
    public class HostedImporterService : ElasticIntervalTaskBasedServiceBase
    {
        #region Initialization and members

        private RedisConnection connRedis;
        private CI.DataAccess.Repository ciRepo;

        private MongoConnection connMongo;
        private DC.DataAccess.Repository dcRepo;

        public HostedImporterService()
            : base(new TimeSpan(0, 3, 0), new TimeSpan(0, 15, 0), new TimeSpan(0, 1, 0))
        {
            Logger.Log("DataCenter.Eyes.TestRuns.SetInterval", true)
                .Append("Default", base.DefaultInterval.ToString())
                .Append("Max", base.MaxInterval.ToString())
                .Append("Step", base.Step.ToString())
                .SubmitAsDebug();

            string redis = ConfigurationManager.AppSettings.Get("CI");
            this.connRedis = new RedisConnection(redis);

            if (this.connRedis.IsConnected)
            {
                this.ciRepo = new CI.DataAccess.Repository(this.connRedis.DB);
            }

            string dataServer = ConfigurationManager.AppSettings.Get("DataCenter");
            this.connMongo = new MongoConnection(dataServer);

            if (this.connMongo.IsConnected)
            {
                this.dcRepo = new DataAccess.Repository(this.connMongo.Client);
            }

            if (this.ciRepo != null && this.dcRepo != null)
            {
                Logger.Log("DataCenter.Eyes.Importer.Connect", true).SubmitAsInfo();
            }
            else
            {
                Logger.Log("DataCenter.Eyes.Importer.Connect", false)
                    .Append("CI", this.ciRepo != null ? "Connected" : "Disconnected")
                    .Append("DataCenter", this.dcRepo != null ? "Connected" : "Disconnected")
                    .SubmitAsError();
                Logger.Log().Append("CI", redis).Append("DataServer", dataServer).SubmitAsDebug();
            }
        }

        #endregion

        protected override bool DoWorkWithResult()
        {
            bool result = false;
            try
            {
                result = ImportTestRuns();
            }
            catch (Exception e)
            {
                Logger.LogException("DataCenter.Eyes.TestRuns.Loop", e);
            }

            Logger.Log()
                .Append("Event", "DataCenter.Eyes.TestRuns.Loop")
                .Append("Result", result.ToString())
                .Append("Interval", base.Interval.ToString())
                .SubmitAsDebug();

            return result;
        }

        private bool ImportTestRuns()
        {
            var allRunsIds = this.ciRepo.GetAllTestRunsIds();
            var runningRuns = this.ciRepo.GetRunningTestRunsIds();
            if (runningRuns == null)
            {
                runningRuns = new string[] { };
            }

            var stopWatch = new System.Diagnostics.Stopwatch();

            if (allRunsIds != null && allRunsIds.Length > 0)
            {
                Logger.Log("DataCenter.Eyes.ReadTestRuns", true).Append("Count", allRunsIds.Length.ToString()).SubmitAsDebug();

                int savedRuns = 0;
                int archivedRuns = 0;

                foreach (var runId in allRunsIds)
                {
                    try
                    {
                        stopWatch.Reset();

                        var run = this.ciRepo.GetTestRun(runId);
                        var result = this.ciRepo.GetTestRunResult(runId);

                        if (run != null && result != null && result.Results != null)
                        {
                            #region Import
                            var count = this.dcRepo.GetTestRunResultsCount(run.Identifier);
                            result = Distinct(run, result);

                            if (runningRuns.Contains(runId))
                            {
                                result = SkipRunningTestCases(result);
                            }
                            
                            if (count < result.Results.Count)
                            {
                                ImportTestRun(run, result, count, stopWatch);
                                savedRuns++;
                                //Console.WriteLine("[{0}/{1}\t{2}\t{3} results]", savedRuns, allRunsIds.Length, runId, result.Results.Count);
                            }
                            else if (!runningRuns.Contains(runId))
                            {
                                // Completed, try to fix Completed Time in database
                                var pair = this.dcRepo.GetTestRun(runId);
                                if (pair.Key != null && pair.Key.CompletedTime < pair.Key.StartedTime)
                                {
                                    // If completed time hasn't been saved
                                    if (run.CompletedTime < run.StartedTime)
                                    {
                                        // Fix invalid completed time for completed run
                                        run.CompletedTime = DateTime.UtcNow;
                                    }

                                    var status = this.ciRepo.GetTestRunStatus(runId);
                                    if (status != null)
                                    {
                                        this.dcRepo.UpdateTestRun(run, status);
                                    }
                                }

                                if (run.CompletedTime < run.StartedTime && pair.Key != null && pair.Key.CompletedTime >= pair.Key.StartedTime)
                                {
                                    // If completed time has been saved and completed time in Redis is invalid 
                                    // Fix invalid completed time, for clean purpose
                                    run.CompletedTime = pair.Key.CompletedTime;
                                }
                            }
                            #endregion

                            #region Clean
                            if (!runningRuns.Contains(runId))
                            {
                                // Completed
                                if (run.Type != "PostCheckin" && run.Type != "PostCheckinForPCR_R_2_6_0")
                                {
                                    if (DateTime.UtcNow.AddHours(-12) >= run.CompletedTime)
                                    {
                                        var status = this.ciRepo.GetTestRunStatus(runId);

                                        Archive2Xml(runId, run, status, result);
                                        RemoveTestRunFromRedis(this.connRedis.Accessor, runId);

                                        archivedRuns++;
                                    }
                                }
                                else
                                {
                                    if (DateTime.UtcNow.AddHours(-72) >= run.CompletedTime)
                                    {
                                        var status = this.ciRepo.GetTestRunStatus(runId);

                                        Archive2Xml(runId, run, status, result);
                                        RemoveTestRunFromRedis(this.connRedis.Accessor, runId);

                                        archivedRuns++;
                                    }
                                }

                            }
                            #endregion
                        }
                    }
                    catch (Exception e)
                    {
                        var ex = new ExceptionEvent("DataCenter.Eyes.ImportTestRun", e);
                        ex.Fields.Add("TestRun", runId);
                        Logger.LogException(ex);
                    }
                }               

                Logger.Log("DataCenter.Eyes.ImportTestRuns", true)
                    .Append("ReadRuns", allRunsIds.Length.ToString())
                    .Append("SavedRuns", savedRuns.ToString())
                    .Append("ArchivedRuns", archivedRuns.ToString())
                    .SubmitAsDebug();

                return savedRuns > 0;
            }

            return false;
        }

        #region Helpers

        private void ImportTestRun(CI.Interface.TestRun run, CI.Interface.TestRunResult result, long resultCountInDb, System.Diagnostics.Stopwatch stopWatch)
        {
            stopWatch.Start();
            try
            {
                var status = this.ciRepo.GetTestRunStatus(run.Identifier);
                this.dcRepo.ImportTestRunWithResults(run, status, result);
                var count = this.dcRepo.GetTestRunResultsCount(run.Identifier);

                Logger.Log("DataCenter.Eyes.ImportTestRun", true)
                    .Append("TestRun", run.Identifier)
                    .Append("RunType", run.Type)
                    .Append("ExistCount", resultCountInDb.ToString())
                    .Append("ReadCount", result.Results.Count.ToString())
                    .Append("AddedCount", (count - resultCountInDb).ToString())
                    .SubmitAsInfo();
            }
            catch (Exception e)
            {
                var ex = new ExceptionEvent("DataCenter.Eyes.ImportTestRun", e);
                if (run != null)
                {
                    ex.Fields.Add("TestRun", run.Identifier);
                }
                Logger.LogException(ex);
            }
            finally
            {
                stopWatch.Stop();
            }

            Logger.Log("DataCenter.Eyes.ImportTestRun", true).Append("TestRun", run.Identifier).Append("Elapsed_ms", stopWatch.ElapsedMilliseconds.ToString()).SubmitAsDebug();
        }

        private static CI.Interface.TestRunResult Distinct(CI.Interface.TestRun run, CI.Interface.TestRunResult result)
        {
            int changeset = 0;
            if (run is CI.Interface.TestRunWithChangeSet)
            {
                changeset = (run as CI.Interface.TestRunWithChangeSet).ChangeSet;
            }

            var names = result.Results.Select(r => r.TestCase.FullName).Distinct().ToArray();
            var newResult = new CI.Interface.TestRunResult()
            {
                TestRun = result.TestRun
            };

            foreach (var name in names)
            {
                var first = result.Results.First(r => r.TestCase.FullName.Equals(name));
                first.TestRun = result.TestRun;
                first.ChangeSet = changeset;

                newResult.Results.Add(first);
            }

            return newResult;
        }

        private static CI.Interface.TestRunResult SkipRunningTestCases(CI.Interface.TestRunResult result)
        {
            var newResult = new CI.Interface.TestRunResult()
            {
                TestRun = result.TestRun
            };

            foreach(var r in result.Results)
            {
                if (!r.Passed && !string.IsNullOrEmpty(r.Message) && r.Message.Contains(string.Format("RUNNING_ON {0}", r.TestMachineName)))
                {
                    if (r.Message.Length > 57)
                    {
                        Logger.Log()
                            .Append("Event", "DataCenter.Eyes.SkipRunningTestCase")
                            .Append("TestRun", result.TestRun)
                            .Append("TestCase", r.TestCase.FullName)
                            .Append("Message", string.Format("\"{0}\"", r.Message.Replace(Environment.NewLine, "")))
                            .SubmitAsDebug();
                    }
                }
                else
                {
                    newResult.Results.Add(r);
                }
            }

            return newResult;
        }

        private static void Archive2Xml(string id, CI.Interface.TestRun run, CI.Interface.TestRunStatus status, CI.Interface.TestRunResult result)
        {
            try
            {
                var archive = new Archive2Xml.ArchivedTestRun(id, run, status, result);

                if (!System.IO.Directory.Exists("TestRuns.Archived"))
                {
                    System.IO.Directory.CreateDirectory("TestRuns.Archived");
                }

                string folder = System.IO.Path.Combine("TestRuns.Archived", run.StartedTime.ToString("yyyyMMdd"));
                if (!System.IO.Directory.Exists(folder))
                {
                    System.IO.Directory.CreateDirectory(folder);
                }

                string path = System.IO.Path.Combine(folder, archive.FileName);
                archive.Save(path);

                Logger.Log("DataCenter.Eyes.ArchiveTestRun", true)
                                        .Append("TestRun", run.Identifier)
                                        .Append("StartedTime", run.StartedTime.ToString("O"))
                                        .Append("CompletedTime", run.CompletedTime.ToString("O"))
                                        .Append("ArchivedFile", string.Format("\"{0}\"", path))
                                        .SubmitAsInfo();
            }
            catch { }
        }

        private static void RemoveTestRunFromRedis(RedisAccessor accessor, string runId)
        {
            try
            {
                accessor.DB.SetRemove("TestRuns:Running", runId);

                accessor.DB.KeyDelete(string.Format("TestRun:{0}", runId));
                accessor.DB.KeyDelete(string.Format("TestRunStatus:{0}", runId));
                accessor.DB.KeyDelete(string.Format("TestRunResult:{0}", runId));                
            }
            catch { }
        }

        #endregion
    }
}
