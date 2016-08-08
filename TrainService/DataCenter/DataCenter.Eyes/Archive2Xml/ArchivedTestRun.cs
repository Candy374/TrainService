using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using ContinuousIntegration.Interface;

namespace DataCenter.Eyes.Archive2Xml
{
    public class ArchivedTestRun : ArchivedObject
    {
        private string Id;

        public ArchivedTestRun(string id, TestRun run, TestRunStatus status, TestRunResult result)
            : base("TestRun")
        {
            this.Id = id;

            if (run != null)
            {
                this.ArchiveTestRunInfo(run);
                this.ArchiveFiles(run.Flies);
            }
            
            if (status != null)
            {
                this.ArchiveTestRunStatus(status);
            }

            if (result != null)
            {
                this.ArchiveTestRunResult(result);
            }
        }

        public override string FileName
        {
            get 
            {
                return string.Format("TestRun.{0}.xml", this.Id);
            }
        }

        private void ArchiveTestRunInfo(TestRun run)
        {
            this.AddTextElement(this.Root, "Identifier", run.Identifier);
            this.AddTextElement(this.Root, "Owner", run.Owner);
            this.AddTextElement(this.Root, "Build", run.Build);
            this.AddTextElement(this.Root, "Type", run.Type);
            this.AddCDATAElement(this.Root, "DropSourceLocation", run.DropSourceLocation);

            if (run is TestRunWithBuildUri)
            {
                this.AddCDATAElement(this.Root, "BuildUri", (run as TestRunWithBuildUri).BuildUri.ToString());
                this.AddCDATAElement(this.Root, "BaselineVersion", (run as TestRunWithBuildUri).BaselineVersion.ToString());
            }
            else if (run is TestRunWithChangeSet)
            {
                this.AddTextElement(this.Root, "ChangeSet", (run as TestRunWithChangeSet).ChangeSet.ToString());
                if ((run as TestRunWithChangeSet).IsFullTestRun)
                {
                    this.AddTextElement(this.Root, "IsFullTestRun", "True");
                }
            }

            this.AddTextElement(this.Root, "StartedTime", run.StartedTime.ToString("O"));
            this.AddTextElement(this.Root, "CompletedTime", run.CompletedTime.ToString("O"));
        }

        private void ArchiveFiles(string[] files)
        {
            var element = this.Document.CreateElement("Files");
            this.Root.AppendChild(element);

            if (files != null && files.Length > 0)
            {
                foreach(var f in files)
                {
                    this.AddCDATAElement(element, "File", f);
                }
            }
        }

        private void ArchiveTestRunStatus(TestRunStatus status)
        {
            var element = this.Document.CreateElement("Workflows");
            element.SetAttribute("Main", status.MainWorkFlowId);
            this.Root.AppendChild(element);

            foreach(var w in status.CompletedTasks)
            {
                var xmlWF = this.AddTextElement(element, "Workflow", w.Value);
                xmlWF.SetAttribute("Id", w.Key);
                xmlWF.SetAttribute("Status", "Completed");                
            }

            foreach (var w in status.AbortedTasks)
            {
                var xmlWF = this.AddTextElement(element, "Workflow", w.Value);
                xmlWF.SetAttribute("Id", w.Key);
                xmlWF.SetAttribute("Status", "Aborted");
            }
        }

        private void ArchiveTestRunResult(TestRunResult result)
        {
            var element = this.Document.CreateElement("Results");
            var passed = result.Results.Count(r => r.Passed);

            element.SetAttribute("Total", result.Results.Count.ToString());
            element.SetAttribute("Passed", passed.ToString());
            
            this.Root.AppendChild(element);

            foreach(var r in result.Results)
            {
                var xmlR = this.AddCDATAElement(element, "Result", r.Message);                
                xmlR.SetAttribute("Testcase", r.TestCase.FullName);
                xmlR.SetAttribute("Owner", r.TestCase.Owner);
                xmlR.SetAttribute("Outcome", r.Passed ? "Passed" : "Failed");
                xmlR.SetAttribute("Duration", r.Duration.Ticks.ToString());
                xmlR.SetAttribute("Machine", r.TestMachineName);
            }
        }
    }
}
