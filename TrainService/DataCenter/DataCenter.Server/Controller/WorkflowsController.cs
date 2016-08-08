using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DataCenter.Server.Controller
{
    [EnableCors("*", "*", "GET")]
    public class WorkflowsController : ApiController
    {
        public IEnumerable<object> Get(string id) 
        {
            try
            {
                var workflow = WorkflowService.Instance.GetWorkflow(id);
                if (workflow != null)
                {
                    return workflow.EmbeddedWorkflows.Select(e => new
                    {
                        Id = e.Information.ID,
                        Name = e.Information.Name,
                        State = e.Context.State.ToString(),
                        Machine = e.Context.GetVariable<string>("MachineName"),
                        TotalTasksCount = e.Context.GetVariable<string>("TotalTasksCount"),
                        TaskIndex = e.Context.GetVariable<string>("TaskIndex"),
                        TestDll = e.Context.GetVariable<string>("TestDll"),
                        Activities = e.Activities.Select(a => new 
                        {
                            Name = a.Value.Name,
                            Status = GetActivityStatus(a.Value),
                            StartTime = a.Value.StartTime.Equals(DateTime.MinValue) ? "" : a.Value.StartTime.ToString("yyyy-MM-dd HH:mm:ss"),
                            EndTime = a.Value.EndTime.Equals(DateTime.MinValue) ? "" : a.Value.EndTime.ToString("yyyy-MM-dd HH:mm:ss"),
                            Msg = Utility.FormatMessage(a.Value.ExceptionMessage),
                            Order = a.Key
                        })
                    });
                }
            }
            catch {}

            return new object[] { };
        }

        private static string GetActivityStatus(Nebula.WorkFlow.Interface.Activity activity)
        {
            if (!string.IsNullOrEmpty(activity.ExceptionMessage))
            {
                return "Error";
            }
            if (activity.StartTime == DateTime.MinValue)
            {
                if (activity.EndTime == DateTime.MinValue)
                {
                    return "NotStart";
                }
                else
                {
                    return "Error";
                }
            }
            else
            {
                if (activity.EndTime == DateTime.MinValue)
                {
                    return "Running";
                }
                else
                {
                    return "Completed";
                }
            }
        }

    }
}
