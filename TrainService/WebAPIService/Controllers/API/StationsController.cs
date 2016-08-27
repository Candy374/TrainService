﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebAPIService.Controllers
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api")]
    public class StationsController : ApiController
    {
        [Route("Stations")]
        public IEnumerable<object> Get()
        {
            return _Get();
        }

        public static IEnumerable<object> _Get()
        {
            return DAL.DalFactory.Stations.CachedTable;
        }

    }
}