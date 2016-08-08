using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using ContinuousIntegration.Interface;

namespace DataCenter.DataAccess
{
    public static class ExtensionMethods
    {
        public static TestCase ToTestCase(this BsonDocument data)
        {
            return new TestCase()
            {
                Identifier = data[Constants.TestCase_Identifier].AsString,
                FullName = data[Constants.TestCase_Fullname].AsString,
                Owner = data[Constants.TestCase_Owner].AsString,
                Scope = data[Constants.TestCase_Scope].AsString,
            };
        }

        public static AssignTo ToAssignment(this BsonDocument data)
        {
            return new AssignTo()
            {
                //Identifier = data[Constants.Assignment_TestCase].AsString,
                AssignToEngineer = data[Constants.Assignment_Engineer].AsString,
                AssignToChangeset = data[Constants.Assignment_ChangeSet].AsInt32,
                Project = data.Contains(Constants.Assignment_ProjectType) ? data[Constants.Assignment_ProjectType].AsString : string.Empty,
                Comment = data[Constants.Assignment_Comment].AsString,
                AssignToTime = data[Constants.Assignment_Time].ToUniversalTime(),
            };
        }

        public static TestCaseResult ToTestCaseResult(this BsonDocument data, TestCase testcase)
        {
            return new TestCaseResult(testcase)
            {
                ChangeSet = data.GetValue(Constants.TestResult_ChangeSet, 0).AsInt32,
                Passed = data.GetValue(Constants.TestResult_Passed, false).AsBoolean,
                TestRun = data[Constants.TestResult_TestRun].IsString ? data[Constants.TestResult_TestRun].AsString : string.Empty,
                TestMachineName = data[Constants.TestResult_MachineName].IsString ? data[Constants.TestResult_MachineName].AsString : string.Empty,
                Message = data[Constants.TestResult_Message].IsString ? data[Constants.TestResult_Message].AsString : string.Empty,
                Duration = new TimeSpan(data.GetValue(Constants.TestResult_Duration, 0L).AsInt64)
            };
        }

        public static TestRun ToTestRun(this BsonDocument data)
        {
            TestRun run = null;

            if (data.Names.Contains(Constants.TestRun_ChangeSet))
            {
                run = new TestRunWithChangeSet() { ChangeSet = data[Constants.TestRun_ChangeSet].AsInt32 };
                if (data.Names.Contains(Constants.TestRun_IsFullTestRun))
                {
                    (run as TestRunWithChangeSet).IsFullTestRun = data[Constants.TestRun_IsFullTestRun].AsBoolean;
                }
            }
            else if (data.Names.Contains(Constants.TestRun_BuildUri))
            {
                run = new TestRunWithBuildUri()
                {
                    BuildUri = data[Constants.TestRun_BuildUri].IsBsonNull ? null : new Uri(data[Constants.TestRun_BuildUri].AsString),
                    BaselineVersion = data.GetValue(Constants.TestRun_BaselineVersion, 0).AsInt32
                };
            }

            if (run != null)
            {
                run.Build = data[Constants.TestRun_Build].AsString;
                run.CompletedTime = data[Constants.TestRun_CompletedTime].ToUniversalTime();
                run.DropSourceLocation = data[Constants.TestRun_DropSourceLocation].AsString;
                run.Identifier = data[Constants.TestRun_Identifier].AsString;
                run.Owner = data[Constants.TestRun_Owner].AsString;
                run.StartedTime = data[Constants.TestRun_StartedTime].ToUniversalTime();
                run.Type = data[Constants.TestRun_Type].AsString;
                run.Flies = data.Names.Contains(Constants.TestRun_Flies) && data[Constants.TestRun_Flies] != BsonNull.Value ? data[Constants.TestRun_Flies].AsBsonArray.Values.Select(f => f.AsString).ToArray() : new string[] {};
            }

            return run;
        }

        public static TestRunStatus ToTestStatus(this BsonDocument data)
        {
            TestRunStatus status = null;

            if (data.Names.Contains(Constants.TestRun_Identifier) && data.Names.Contains(Constants.TestRun_MainWorkflow))
            {
                status = new TestRunStatus() {
                    Identifier = data[Constants.TestRun_Identifier].AsString, 
                    MainWorkFlowId = data[Constants.TestRun_MainWorkflow].AsString
                };
            }            

            if (status != null)
            {
                if (data.Names.Contains(Constants.TestRun_Workflows) && data[Constants.TestRun_Workflows] != BsonNull.Value)
                {
                    var array = data[Constants.TestRun_Workflows].AsBsonArray;
                    foreach (var d in array)
                    {
                        if (d.IsBsonDocument)
                        {
                            var id = d.AsBsonDocument[Constants.TestRun_Workflow_Identifier].AsString;
                            var name = d.AsBsonDocument[Constants.TestRun_Workflow_Name].AsString;
                            var stat = d.AsBsonDocument[Constants.TestRun_Workflow_Status].AsString;

                            if (stat == "Completed")
                            {
                                status.CompletedTasks.Add(id, name);
                            }
                            else
                            {
                                status.AbortedTasks.Add(id, name);
                            }
                        }
                    }
                }
            }

            return status;
        }
    }
}
