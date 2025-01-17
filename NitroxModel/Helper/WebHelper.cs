﻿using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using NitroxModel.Logger;

namespace NitroxModel.Helper
{
    public static class WebHelper
    {
        //"Hacky way" to get the public IP
        //Should definitely be reworked.
        public static string GetPublicIP()
        {
            WebRequest req = WebRequest.Create("http://checkip.dyndns.org");

            using (StreamReader sr = new StreamReader(req.GetResponse().GetResponseStream()))
            {
                return sr.ReadToEnd().Trim().Split(':')[1].Substring(1).Split('<')[0];
            }
        }

        public static Version GetNitroxLatestVersion()
        {
            HttpWebRequest req = WebRequest.Create("https://nitrox.rux.gg/api/version/latest") as HttpWebRequest;
            req.Method = "GET";
            req.UserAgent = "Nitrox";
            req.ContentType = "application/json";
            req.Timeout = 5000;

            try
            {
                using (StreamReader sr = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    string json = sr.ReadToEnd();
                    Regex rx = new Regex(@"""version"":""([^""]*)""");
                    Match match = rx.Match(json);

                    if (match.Success)
                    {
                        return new Version(match.Groups[1].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "WebHelper : Error while fetching nitrox latest version on GitHub");
            }

            return new Version();
        }
    }
}
