using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DataCenter.Server.Controller
{
    [EnableCors("*", "*", "GET")]
    [RoutePrefix("api")]
    public class TestCasesController : ApiController
    {
        [Route("TestCases/{projectType}/{fullname}")]
        public object Get(string projectType, string fullname)
        {
            var h = DataService.Instance.GetTestCaseByName(fullname, projectType);

            if (h != null)
            {
                return new
                {
                    Identifier = h.TestCase.Identifier,
                    Fullname = h.TestCase.FullName,
                    Owner = NameService.GetCoreId(h.TestCase.Owner),
                    OwnerName = NameService.Instance.GetUserName(h.TestCase.Owner),
                    History = h.History.Select(r => new
                    {
                        Version = r.ChangeSet,
                        Passed = r.Passed,
                        TestRun = r.TestRun,
                        RunType = projectType,
                        Message = !string.IsNullOrEmpty(r.Message) ? Utility.FormatMessage(r) : string.Empty
                    }).ToArray()
                };
            }

            return new { Identifier = "Unknown", Fullname = fullname };
        }
    }
}
