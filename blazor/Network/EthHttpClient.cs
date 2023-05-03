using System.Collections.Generic; 
using System.Net.Http;
using System.Text.Json; 
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UptraderEthBlazor
{
    /// <summary>
    /// 
    /// </summary>
    public class EthHttpClient 
    {
        /// <summary>
        /// 
        /// </summary>
        private static readonly HttpClient client = new HttpClient();
        
        /// <summary>
        /// 
        /// </summary>
        public static string Get(string requestUri)
        {
            string responseString = string.Empty; 
            try
            {
                Task.Run(async () => {
                    responseString = await client.GetStringAsync(requestUri);
                    System.Console.WriteLine(responseString);
                }).Wait();
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
            return responseString; 
        }

        /// <summary>
        /// 
        /// </summary>
        public static string Post(string requestUri, Dictionary<string, string> values) 
        {
            string responseString = string.Empty; 
            try
            {
                Task.Run(async () => {
                    string jsonStr = System.Text.Json.JsonSerializer.Serialize(values); 
                    var response = await client.PostAsync(requestUri, new StringContent(jsonStr));
                    responseString = await response.Content.ReadAsStringAsync();
                }).Wait();
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
            return responseString; 
        }
    }
}
