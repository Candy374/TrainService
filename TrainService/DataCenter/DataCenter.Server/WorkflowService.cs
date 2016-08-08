using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using Nebula.Log;
using Nebula.Redis;
using Nebula.WorkFlow.Interface;
using Nebula.WorkFlow.DataAccess;

namespace DataCenter.Server
{
    class WorkflowService
    {
        #region Initialization and members

        private RedisConnection connRedis;
        private Repository Repository;

        public static WorkflowService Instance { get; private set; }

        static WorkflowService()
        {
            Instance = new WorkflowService();
        }

        private WorkflowService()            
        {
            string dataServer = ConfigurationManager.AppSettings.Get("WorkflowDatabase");
            this.connRedis = new RedisConnection(dataServer);

            if (this.connRedis.IsConnected)
            {
                this.Repository = new Repository(this.connRedis.DB);
            }

            if (this.Repository != null)
            {
                Logger.Log("DataCenter.Server.WorkflowService.Connect", true)
                    .Append("Server", dataServer)
                    .SubmitAsInfo();
            }
            else
            {
                Logger.Log("DataCenter.Server.WorkflowService.Connect", false)
                    .Append("Status", this.Repository != null ? "Connected" : "Disconnected")
                    .Append("Server", dataServer)
                    .Append("Expection", this.connRedis.ConnectionException != null ? this.connRedis.ConnectionException.Message : "")
                    .SubmitAsError();
            }
        }

        #endregion

        #region Getters

        public Object.WorkflowInformation GetWorkflow(string workflowId)
        {
            try
            {
                var workflow = this.Repository.Get(workflowId);
                if (workflow != null)
                {
                    var activity = this.Repository.GetActivity(workflow.ID, 0);
                    if (activity != null && activity is EmbeddedWorkFlowsActivity)
                    {
                        var wi = new Object.WorkflowInformation(workflow, this.Repository.GetContext(workflow.ID), null);

                        var embeddedWFs = (activity as EmbeddedWorkFlowsActivity).EmbeddedWorkFlows;
                        foreach(var wf in embeddedWFs)
                        {
                            wi.AddEmbeddedWorkflow(
                                new Object.WorkflowInformation(
                                    this.Repository.Get(wf.ID),
                                    this.Repository.GetContext(wf.ID),
                                    this.Repository.GetActivitys(wf.ID))
                                );
                        }

                        return wi;
                    }
                    else
                    {
                        return new Object.WorkflowInformation(workflow, this.Repository.GetContext(workflow.ID), this.Repository.GetActivitys(workflow.ID));
                    }
                }
            }
            catch { }

            return null;
        }

        public string[] GetIds(WorkFlowState state)
        {
            return this.Repository.Get(state);
        }

        #endregion
    }
}
