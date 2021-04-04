using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace MavcaDetection.Extensions
{
    public static class HttpClientExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source">Properties Dictionary</param>
        /// <returns></returns>
        public static string ToQueryParamsString(this IDictionary<string, string> source)
        {
            // kvp: key value pair
            //TODO - Check
            return string.Join("&", source.Select(kvp => string.Format("{0}={1}",
                HttpUtility.UrlEncode(kvp.Key), HttpUtility.UrlEncode(kvp.Value))));
        }

        public static string ToQueryParamsString(this NameValueCollection source)
        {
            return string.Join("&", source.Cast<string>().Select(key => string.Format("{0}={1}",
                HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(source[key]))));
        }

        public static Uri AddQueryParams(this Uri uri, string query)
        {
            var ub = new UriBuilder(uri);
            ub.Query = string.IsNullOrEmpty(uri.Query) ? query : string.Join("&", uri.Query.Substring(1), query);
            return ub.Uri;
        }

        public static Uri AddQueryParams(this Uri uri, IEnumerable<string> query)
        {
            return uri.AddQueryParams(string.Join("&", query));
        }

        public static Uri AddQueryParams(this Uri uri, string key, string value)
        {
            return uri.AddQueryParams(string.Join("=", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value)));
        }

        public static Uri AddQueryParams(this Uri uri, params KeyValuePair<string, string>[] kvps)
        {
            return uri.AddQueryParams(kvps.Select(kvp => string.Join("=", HttpUtility.UrlEncode(kvp.Key), HttpUtility.UrlEncode(kvp.Value))));
        }

        public static Uri AddQueryParams(this Uri uri, IDictionary<string, string> kvps)
        {
            return uri.AddQueryParams(kvps.Select(kvp => string.Join("=", HttpUtility.UrlEncode(kvp.Key), HttpUtility.UrlEncode(kvp.Value))));
        }

        public static Uri AddQueryParams(this Uri uri, NameValueCollection nvc)
        {
            return uri.AddQueryParams(nvc.AllKeys.SelectMany(nvc.GetValues,
                (key, value) => string.Join("=", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value))));
        }

    }
}
