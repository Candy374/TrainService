using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAPIService.Controllers
{
    public class TokenVerifyController : Controller
    {
        // GET: TokenVerify
        public string Index(string signature, string timestamp, string nonce, string echostr)
        {
            List<string> args = new List<string> { timestamp, "werfc4567UHFygh7t", nonce };
            args.Sort();
            var str1 = string.Format("{0}{1}{2}", args[0],args[1],args[2]);
            var encoded = CommonUtilities.EncryptHelper.EncryptToSHA1(str1);
            if (encoded.Equals(signature,StringComparison.OrdinalIgnoreCase))
            {
                return echostr;
            }

            return "";
        }
    }
}