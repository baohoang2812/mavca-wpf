using MavcaDetection.Extensions;
using MavcaDetection.Requests;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MavcaDetection.Services
{
    public interface IBaseService
    {

    }

    public class BaseService : IBaseService
    {
        private readonly HttpClient _client;
        protected Uri EndPoint { get; set; }
        protected const string BaseURL = "https://mavcahub-api.azurewebsites.net/v1/";

        public BaseService()
        {
            _client = new HttpClient();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestUri">Built Get Uri</param>
        /// <returns></returns>
        public virtual async Task<T> Get<T>(BaseGetRequestDTO model)
            where T : class, new()
        {
            var requestUri = BuildRequestUri(model);

            T result = null;
            var resp = await _client.GetAsync(requestUri);
            if (resp.IsSuccessStatusCode)
            {
                result = await resp.Content.ReadAsAsync<T>();
            }
            return result;
        }

        public void SetAuthorizationHeader(string accessToken)
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);
        }

        public virtual async Task<T> Get<T>()
            where T : class, new()
        {
            T result = null;

            var resp = await _client.GetAsync(EndPoint);
            if (resp.IsSuccessStatusCode)
            {
                result = await resp.Content.ReadAsAsync<T>();
            }
            return result;
        }

        public virtual async Task<T> ReadResponse<T>(HttpResponseMessage response)
            where T : class
        {
            return await response.Content.ReadAsAsync<T>();
        }

        public virtual async Task<HttpResponseMessage> PostAsync(BasePostRequestDTO request)
        {
            return await _client.PostAsJsonAsync(EndPoint, request);
        }

        public virtual async Task<T> UpdateAsync<T>(BaseUpdateRequestDTO model)
            where T : class, new()
        {
            var resp = await _client.PutAsJsonAsync($"{EndPoint}/{model.Id}", model);

            // Deserialize the updated product from the response body.
            var result = await resp.Content.ReadAsAsync<T>();
            return result;
        }

        public virtual async Task<HttpStatusCode> DeleteAsync(string id)
        {
            var resp = await _client.DeleteAsync($"{EndPoint}/{id}");
            return resp.StatusCode;
        }

        public virtual Uri BuildRequestUri(BaseGetRequestDTO model)
        {
            var queryParamStr = model.AsDictionary().ToQueryParamsString();
            return new Uri(EndPoint.AbsoluteUri).AddQueryParams(queryParamStr);
        }
    }
}
