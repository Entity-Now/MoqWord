using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Helpers
{
    public static class HttpHelper
    {

        public static async Task<HttpResponseMessage> Get(this HttpClient httpClient, string url)
        {
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return response;
            }
            throw new Exception(await response.Content.ReadAsStringAsync());
        }
        public static Task<HttpResponseMessage> PostIsForm(this HttpClient client, string url, IEnumerable<KeyValuePair<string, string>> body)
        {
            return client.PostAsync(url, new FormUrlEncodedContent(body));
        }
        public static Task<HttpResponseMessage> PostIsString(this HttpClient client, string url, string body, string meadia = "text/plain")
        {
            return client.PostAsync(url, new StringContent(body, mediaType: new MediaTypeHeaderValue(meadia)));
        }
        public static Task<HttpResponseMessage> PostIsJson(this HttpClient client, string url, object body, string meadia = "application/json")
        {
            return client.PostAsync(url, JsonContent.Create(body, mediaType: new MediaTypeHeaderValue(meadia)));
        }
        public static Task<HttpResponseMessage> PostIsStream(this HttpClient client, string url, Stream body)
        {
            return client.PostAsync(url, new StreamContent(body));
        }
    }
}
