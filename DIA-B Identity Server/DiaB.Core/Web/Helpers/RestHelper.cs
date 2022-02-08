// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     RestHelper.cs
// Created Date:  2018/10/12 2:59 PM
// ------------------------------------------------------------------------

using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiaB.Core.Web.Helpers
{
    public static class RestHelper
    {
        #region GET

        public static async Task<IRestResponse<T>> GetAsync<T>(string url, Dictionary<string, object> parameters = null, IDictionary<string, string> headers = null)
        {
            return await new RestClient(url).ExecuteAsync<T>(BuildGetRequest(parameters, headers));
        }


        public static async Task<IRestResponse> GetAsync(string url, Dictionary<string, object> parameters = null, IDictionary<string, string> headers = null)
        {
            return await new RestClient(url).ExecuteAsync(BuildGetRequest(parameters, headers));
        }

        #endregion

        #region POST

        public static async Task<IRestResponse<T>> PostJsonAsync<T>(string url, object body, Dictionary<string, string> headers = null)
        {
            return await new RestClient(url).ExecuteAsync<T>(BuildJsonRequest(Method.POST, body, headers));
        }

        public static async Task<IRestResponse> PostJsonAsync(string url, object body, Dictionary<string, string> headers = null)
        {
            return await new RestClient(url).ExecuteAsync(BuildJsonRequest(Method.POST, body, headers));
        }

        #endregion

        #region PUT

        public static async Task<IRestResponse<T>> PutJsonAsync<T>(string url, object body, Dictionary<string, string> headers = null)
        {
            return await new RestClient(url).ExecuteAsync<T>(BuildJsonRequest(Method.PUT, body, headers));
        }

        public static async Task<IRestResponse> PutJsonAsync(string url, object body, Dictionary<string, string> headers = null)
        {
            return await new RestClient(url).ExecuteAsync(BuildJsonRequest(Method.PUT, body, headers));
        }

        #endregion

        #region Common

        public static async Task<IRestResponse> SendAsync(string url, RestRequest request)
        {
            return await new RestClient(url).ExecuteAsync(request);
        }

        public static async Task<IRestResponse<T>> SendAsync<T>(string url, RestRequest request)
        {
            return await new RestClient(url).ExecuteAsync<T>(request);
        }

        #endregion

        private static RestRequest BuildGetRequest(Dictionary<string, object> parameters = null, IDictionary<string, string> headers = null)
        {
            var request = new RestRequest(Method.GET);

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    request.AddParameter(param.Key, param.Value);
                }
            }

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.AddHeader(header.Key, header.Value);
                }
            }

            return request;
        }

        private static RestRequest BuildJsonRequest(Method method, object body, IDictionary<string, string> headers = null)
        {
            var request = new RestRequest(method);

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.AddHeader(header.Key, header.Value);
                }
            }

            request.AddJsonBody(body);

            return request;
        }
    }
}
