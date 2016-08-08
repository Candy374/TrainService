using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using ContinuousIntegration.Interface;

namespace DataCenter.Eyes.XmlData
{
    public class Reader
    {
        static XmlSerializer Serializer = new XmlSerializer(typeof(XmlData.TestRun));
        public static void Read(string xmlFilePath, out ContinuousIntegration.Interface.TestRun run, out TestRunStatus status, out TestRunResult result)
        {
            run = null;
            status = null;
            result = null;

            using (var fs = new FileStream(xmlFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                
                var runXmlData = Serializer.Deserialize(fs) as XmlData.TestRun;

                switch (runXmlData.Type)
                {
                    case "Common2GatedTest":
                    case "RMShelveTest":
                    case "TestDataCreation":
                        run = new TestRunWithBuildUri()
                        {
                            BuildUri = string.IsNullOrEmpty(runXmlData.BuildUri) ? null : new Uri(runXmlData.BuildUri),
                            BaselineVersion = 0
                        };
                        break;
                    case "PostCheckin":
                    case "RmcDailyRun":
                        run = new TestRunWithChangeSet()
                        {
                            ChangeSet = runXmlData.ChangeSet
                        };
                        break;
                }

                if (run != null)
                {
                    run.Build = runXmlData.Build;
                    run.CompletedTime = runXmlData.CompletedTime;
                    run.DropSourceLocation = runXmlData.DropSourceLocation;
                    run.Flies = runXmlData.Files;
                    run.Identifier = runXmlData.Identifier;
                    run.Owner = runXmlData.Owner;
                    run.StartedTime = runXmlData.StartedTime;
                    run.Type = runXmlData.Type;

                    status = new TestRunStatus() { Identifier = run.Identifier };
                    if (runXmlData.Workflows != null)
                    {
                        status.MainWorkFlowId = runXmlData.Workflows.Main;

                        if (runXmlData.Workflows.Workflow != null)
                        {
                            foreach (var w in runXmlData.Workflows.Workflow)
                            {
                                if (w.Status.Equals("Completed"))
                                {
                                    status.CompletedTasks.Add(w.Id, w.Value);
                                }
                                else
                                {
                                    status.AbortedTasks.Add(w.Id, w.Value);
                                }
                            }
                        }
                    }

                    result = new TestRunResult() { TestRun = run.Identifier };
                    if (runXmlData.Results != null && runXmlData.Results.Result != null && runXmlData.Results.Result.Length > 0)
                    {
                        foreach (var r in runXmlData.Results.Result)
                        {
                            try
                            {
                                var re = new TestCaseResult()
                                {
                                    TestRun = run.Identifier,
                                    Duration = new TimeSpan(r.Duration),
                                    Passed = r.Outcome.Equals("Passed", StringComparison.InvariantCulture),
                                    TestMachineName = r.Machine,
                                    Message = r.Value
                                };
                                re.TestCase.FullName = r.Testcase;
                                re.TestCase.Scope = r.Testcase.Split(':')[0];

                                if (runXmlData.ChangeSet != 0)
                                {
                                    re.ChangeSet = runXmlData.ChangeSet;
                                }

                                result.Results.Add(re);
                            }
                            catch { }
                        }
                    }                    
                }
            }
        }
    }
}
