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
    public class TagsController : ApiController
    {
        [Route("Tags/{goodsType}")]
        public IEnumerable<object> Get(int goodsType)
        {
            return _Get();
        }

        public static IEnumerable<object> _Get()
        {
            return DAL.DalFactory.Tags.GetTags();
        }
    }
}
