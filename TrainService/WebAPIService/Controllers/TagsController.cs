using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebAPIService.Controllers
{
    [EnableCors("*", "*", "GET")]
    [RoutePrefix("api")]
    public class TagsController : ApiController
    {
        [Route("Tags")]
        public IEnumerable<object> Get()
        {
            return DAL.DalFactory.Tags.GetTags();
        }
    }
}
