﻿using PayPal.Api;
using System.Collections.Generic;

namespace Store.WebUI.Models
{
    public static class PaypalConfiguration
    {
        //these variables will store the clientID and clientSecret
        //by reading them from the web.config
        public readonly static string ClientId;
        public readonly static string ClientSecret;

        static PaypalConfiguration()
        {
            var config = GetConfig();
            ClientId = config["clientId"];
            ClientSecret = config["clientSecret"];
        }

        // getting properties from the web.config
        public static Dictionary<string, string> GetConfig()
        {
            return ConfigManager.Instance.GetProperties();
        }

        private static string GetAccessToken()
        {
            // getting accesstocken from paypal                
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();

            return accessToken;
        }

        public static APIContext GetAPIContext()
        {
            // return apicontext object by invoking it with the accesstoken
            APIContext apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }

    }
}