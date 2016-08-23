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
    [RoutePrefix("api/Provider")]
    public class ProviderController : ApiController
    {
        [Route("Find/ID/{providerId}")]
        public object GetById(uint providerId)
        {
            return DAL.DalFactory.Provider.Search(providerId);
        }

        [Route("Find/Name/{providerName}")]
        public IEnumerable<DAL.Entity.ProviderEntity> GetByName(string providerName)
        {
            return DAL.DalFactory.Provider.Search(providerName);
        }
    }
}
