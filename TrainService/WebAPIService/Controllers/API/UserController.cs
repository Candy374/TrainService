using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebAPIService.Controllers
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        [Route("LastInput/{openId}")]
        public object Get(string openId)
        {
            return new LastUserInput
            {
                Contact = "张三",
                ContactTel = "13812312390",
                TrainNumber = "G123",
                CarriageNumber = "6"
            };
        }

        public class LastUserInput
        {
            public string Contact { get; set; }
            public string ContactTel { get; set; }

            public string TrainNumber { get; set; }
            public string CarriageNumber { get; set; }
        }
    }
}
