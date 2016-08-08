using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCenter.DataAccess
{
    internal class Constants
    {
        public const string TestRun_Identifier = "Identifier";
        public const string TestRun_Owner = "Owner";
        public const string TestRun_Build = "Build";
        public const string TestRun_Type = "Type";
        public const string TestRun_Flies = "Flies";
        public const string TestRun_DropSourceLocation = "DropSourceLocation";
        public const string TestRun_StartedTime = "StartedTime";
        public const string TestRun_CompletedTime = "CompletedTime";
        public const string TestRun_BuildUri = "BuildUri";
        public const string TestRun_ChangeSet = "ChangeSet";
        public const string TestRun_IsFullTestRun = "IsFullTestRun";
        public const string TestRun_BaselineVersion = "BaselineVersion";
        public const string TestRun_MainWorkflow = "MainWorkflow";
        public const string TestRun_Workflows = "Workflows";
        public const string TestRun_Workflow_Identifier = "Identifier";
        public const string TestRun_Workflow_Status = "Status";
        public const string TestRun_Workflow_Name = "Name";

        public const string TestCase_Identifier = "Identifier";
        public const string TestCase_Fullname = "Fullname";
        public const string TestCase_Scope = "Scope";
        public const string TestCase_Owner = "Owner";

        public const string TestResult_TestCase = "TestCase";
        public const string TestResult_TestRun = "TestRun";
        public const string TestResult_TestRunType = "TestRunType";
        public const string TestResult_Fullname = "Fullname";
        public const string TestResult_ChangeSet = "ChangeSet";
        public const string TestResult_Passed = "Passed";
        public const string TestResult_Message = "Message";
        public const string TestResult_MachineName = "MachineName";
        public const string TestResult_Duration = "Duration";

        public const string Assignment_TestCase = "TestCase";
        public const string Assignment_Engineer = "Engineer";
        public const string Assignment_ChangeSet = "ChangeSet";
        public const string Assignment_ProjectType = "ProjectType";
        public const string Assignment_Comment = "Comment";
        public const string Assignment_Time = "Time";
    }
}
