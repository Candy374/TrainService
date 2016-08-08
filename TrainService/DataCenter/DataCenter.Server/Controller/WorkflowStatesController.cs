using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DataCenter.Server.Controller
{
    [EnableCors("*", "*", "GET")]
    public class WorkflowStatesController : ApiController
    {
        public IEnumerable<string> Get(string id) 
        {
            try
            {
                Nebula.WorkFlow.Interface.WorkFlowState state;
                if (Enum.TryParse<Nebula.WorkFlow.Interface.WorkFlowState>(id, true, out state))
                {
                    return WorkflowService.Instance.GetIds(state);
                }
            }
            catch {}

            return new string[] { };
        }       

    }
}
