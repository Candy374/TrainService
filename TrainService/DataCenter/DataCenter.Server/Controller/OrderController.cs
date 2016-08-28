using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DataCenter.Server.Controller
{
    [EnableCors("*", "*", "POST")]
    [RoutePrefix("api")]
    public class OrderController : ApiController
    {
        [HttpPost, Route("Order")]
        public string Post([FromBody]dynamic value)
        {
            try
            {
                var cart = value.Cart;
                if (cart == null || cart.List == null || cart.List.Count == 0)
                {
                    return "购物车为空";
                }

            }
            catch (Exception ex)
            {
                return "购买失败，请稍后再试";
            }
        }

    }
}
