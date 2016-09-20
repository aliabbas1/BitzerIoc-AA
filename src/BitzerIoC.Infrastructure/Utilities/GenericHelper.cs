using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
namespace BitzerIoC.Infrastructure.Utilities
{
    public class GenericHelper
    {
        /// <summary>
        /// Function removes the url and ? and return the querystring parameters
        /// </summary>
        /// <param name="queryString">like msn.com?p1=a&p2=b</param>
        /// <returns>p1=a,p2=b</returns>
        public static Dictionary<string, string> ParseQueryString(string queryString)
        {
            queryString = WebUtility.UrlDecode(queryString);

            if (queryString.Contains("?"))
            {
                queryString = queryString.Remove(0, queryString.IndexOf('?') + 1);
            }
            Dictionary<string, string> queryParameters = new Dictionary<string, string>();
            string[] querySegments = queryString.Split('&');
            foreach (string segment in querySegments)
            {
                string[] parts = segment.Split('=');
                if (parts.Length > 0)
                {
                    string key = parts[0].Trim(new char[] { '?', ' ' });
                    string val = parts[1].Trim();
                    queryParameters.Add(key, val);

                }
            }

            return queryParameters;
        }

        /// <summary>
        /// Decode Url
        /// </summary>
        /// <param name="url"></param>
        public static string DecodeUrl(string url)
        {
            return WebUtility.UrlDecode(url);
        }

        /// <summary>
        /// Encode provided url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string EncodeUrl(string url)
        {
            return WebUtility.UrlEncode(url);
        }

    }
}
