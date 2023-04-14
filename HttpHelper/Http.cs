using AGVSytemCommonNet6.TASK;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AGVSytemCommonNet6.HttpHelper
{
    public class Http
    {

        public class clsInternalError
        {
            public string ErrorCode { get; set; }
            public string ErrorMessage { get; set; } = string.Empty;
        }

        public static async Task<Tout> PostAsync<Tin, Tout>(string url, Tin data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseJson = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<Tout>(responseJson);
                        return result;
                    }
                    else
                    {
                        throw new HttpRequestException($"Failed to POST to {url}. Response status code: {response.StatusCode}");
                    }
                }

            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"Failed to POST to {url}. {ex.Message}");
            }

        }
        public static async Task<Tin> GetAsync<Tin>(string url)
        {
            string jsonContent = "";
            HttpResponseMessage response = null;
            using (HttpClient client = new HttpClient())
            {
                response = await client.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    jsonContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<Tin>(jsonContent);
                    return result;
                }
                else
                    throw new HttpRequestException($"Failed to GET to {url}({response.StatusCode})");
            }

        }
        public static async Task<(HttpResponseMessage response, string content)> Get(string host, string request_url)
        {
            string jsonContent = "";
            HttpResponseMessage response = null;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(host);

            var task = Task.Run(async () =>
            {
                response = await client.GetAsync(request_url);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    jsonContent = await response.Content.ReadAsStringAsync();
                }
                return (response, jsonContent);
            });
            return await task.ContinueWith(t => t.Result);
        }
    }
}
