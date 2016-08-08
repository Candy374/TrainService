using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nebula.WorkFlow.Interface;

namespace DataCenter.Server.Object
{
    class WorkflowInformation
    {
        public WorkFlow Information { get; private set; }
        public WorkFlowContext Context { get; private set; }
        public SortedList<int, Activity> Activities { get; private set; }
        public IList<WorkflowInformation> EmbeddedWorkflows { get; private set; }

        public WorkflowInformation(WorkFlow information, WorkFlowContext context, SortedList<int, Activity> activities)
        {
            this.Information = information;
            this.Context = context != null ? context : new WorkFlowContext() { WorkFlowID = information.ID };
            this.Activities = activities != null ? activities : new SortedList<int, Activity>();
            this.EmbeddedWorkflows = new List<WorkflowInformation>();
        }

        internal void AddEmbeddedWorkflow(WorkflowInformation embeddedWorkflow)
        {
            this.EmbeddedWorkflows.Add(embeddedWorkflow);
        }
    }
}
