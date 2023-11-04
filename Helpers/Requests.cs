using GoRest_L9.Models;
using Newtonsoft.Json;

namespace GoRest_L9.Helpers
{
    public static class Requests
    {
        public static async Task<HttpResponseMessage> NoBodyReq(HttpClient client, HttpMethod method, string url, string token)
        {
            var message = new HttpRequestMessage(method, url);
            message.Headers.Add("Authorization", token);

            HttpResponseMessage response = await client.SendAsync(message);
            
            return response;
        }
 
        public static async Task<HttpResponseMessage> BodyReq(HttpClient client, HttpMethod method, string url, string token, User user)
        {
            string jsonUser = JsonConvert.SerializeObject(user);

            var message = new HttpRequestMessage(method, url);
            message.Content = new StringContent(jsonUser);
            message.Headers.Add("Accept", "application/json");
            message.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            message.Headers.Add("Authorization", token);

            HttpResponseMessage response = await client.SendAsync(message);
            
            return response;
        }

        public static async Task<HttpResponseMessage> BodyReq(HttpClient client, HttpMethod method, string url, string token, string patchString)
        {
            var message = new HttpRequestMessage(method, url);
            message.Content = new StringContent(patchString);
            message.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            message.Headers.Add("Accept", "application/json");
            message.Headers.Add("Authorization", token);

            HttpResponseMessage response = await client.SendAsync(message);
            
            return response;
        }
    }
}
