using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DataCenter.Server.Controller
{
    [EnableCors("*", "*", "GET")]
    public class NamesController : ApiController
    {
        public string Get(string id) 
        {
            try
            {
                return NameService.Instance.GetUserName(id);
            }
            catch { }

            return id;
        }

    }
}
