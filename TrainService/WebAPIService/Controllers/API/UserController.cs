﻿using CommonUtilities;
using LoggerContract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace WebAPIService.Controllers
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        [Route("LastInput/{openId}")]
        public object Get(string openId)
        {
            Logger.Info("openId=" + openId, "api/User/LastInput/{openId}");
            var user = DAL.DalFactory.Account.GetAccount(openId);
            if (user == null)
            {
                return new LastUserInput
                {

                    Contact = "",
                    TrainNumber = "",
                    CarriageNumber = "",
                    ContactTel = ""
                };
            }

            return new LastUserInput
            {
                Contact = user.LastContactName,
                ContactTel = user.LastContactTel,
                TrainNumber = "",
                CarriageNumber = ""
            };
        }

        [Route("Info/Code/{code}/State/{state}")]
        public string GetOpenIdByCode(string code, string state)
        {
            Logger.Info("code={0}, state={1}".FormatedWith(code, state), "api/User/Info/Code/{code}/State/{state}");
            var openId= _GetOpenIdByCode(code, state);
            Logger.Info("Successful get Open ID={0} via code={1}".FormatedWith(openId, code), "api/User/Info/Code/{code}/State/{state}");

            return openId;
        }

        internal static string _GetOpenIdByCode(string code, string state)
        {
            string url = "https://api.weixin.qq.com/sns/oauth2/access_token" +
              "?appid=wxaf1fff843c641aba&secret=c60530db2bd0ac505c8ce30578bd61f0&" +
              "code={0}&grant_type=authorization_code".FormatedWith(code);
            int status;
            string json;
            json = HttpGet(url, out status);

            if (status == 200)
            {
                dynamic obj = JsonToDynamic(json);
                string accessToken = obj.access_token;
                string refreshToken = obj.refresh_token;
                string openId = obj.openid;
                string scope = obj.scope;
                var getUserInfoUrl = "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN".FormatedWith(accessToken, openId);
                var userInfoData = HttpGet(getUserInfoUrl, out status);
                var userInfo = JsonToDynamic(userInfoData);
                var account = new DAL.Entity.AccountEntity()
                {
                    OpenId = userInfo.openid,
                    City = userInfo.city,
                    Country = userInfo.country,
                    Province = userInfo.province,
                    HeadImgUrl = userInfo.headimgurl,
                    NickName = userInfo.nickname,
                    Sex = userInfo.sex ?? 0
                };

                var existAccount = DAL.DalFactory.Account.GetAccount(openId);
                if (existAccount != null)
                {
                    existAccount.City = account.City;
                    existAccount.Country = account.Country;
                    existAccount.Province = account.Province;
                    existAccount.HeadImgUrl = account.HeadImgUrl;
                    existAccount.NickName = account.NickName;
                    existAccount.Sex = account.Sex;

                    DAL.DalFactory.Account.Update(existAccount);
                }
                else
                {
                    DAL.DalFactory.Account.Add(account);
                }

                return openId;
            }
            else
            {
                LoggerContract.Logger.Error("Request error: Status={0}, input: code={1}, state={2}".FormatedWith(status, code ?? "<null>", state ?? "<null>"));
                return "";
            }
        }



        private static dynamic JsonToDynamic(string json)
        {
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
            dynamic obj = serializer.Deserialize(json, typeof(object));
            return obj;
        }

        private static string HttpGet(string url, out int status)
        {
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Timeout = 10000;
            httpRequest.Method = "GET";
            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            status = 0;
            var content = "";
            using (StreamReader sr = new StreamReader(httpResponse.GetResponseStream(), System.Text.Encoding.UTF8))
            {
                status = (int)httpResponse.StatusCode;
                content = sr.ReadToEnd();
                sr.Close();
            }

            return content;
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
