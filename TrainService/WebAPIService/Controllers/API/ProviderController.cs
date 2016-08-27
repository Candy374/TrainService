using CommonUtilities;
using LoggerContract;
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
            Logger.Info("providerId=" + providerId, "api/Provider/Find/ID/{providerId}");

            return DAL.DalFactory.Provider.Search(providerId);
        }

        [Route("Find/Code/{code}")]
        public int GetProviderIdByCode(string code)
        {
            var openId = UserController._GetOpenIdByCode(code, "");
            var provider = DAL.DalFactory.Provider.GetProviderByOpenId(openId);
            if (provider == null)
            {
                Logger.Error("Get ProviderId failed: OpenId={0}".FormatedWith(openId));
                return -1;
            }

            Logger.Info("Get ProviderId success. OpenId={0}, ProviderId={1}, Name={2}".FormatedWith(openId, provider.ProviderId, provider.Name), "api/Provider/Find/Code/{code}");
            return Convert.ToInt32(provider.ProviderId);
        }

        [Route("Find/Name/{providerName}")]
        public IEnumerable<DAL.Entity.ProviderEntity> GetByName(string providerName)
        {
            Logger.Info("providerName=" + providerName, "api/Provider/Find/Name/{providerName}");
            return DAL.DalFactory.Provider.Search(providerName);
        }
    }
}
