using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        private HttpClient _httpClient;
        public Program()
        {
            //var handler = new WebRequestHandler {ReadWriteTimeout = (int) TimeSpan.FromMinutes(10).TotalMilliseconds};
            //_httpClient = new HttpClient(handler) {Timeout = TimeSpan.FromMinutes(15)};
            _httpClient = new HttpClient() {Timeout = TimeSpan.FromMinutes(15)};
        }
        public static void Main(string[] args)
        {
            Debug.WriteLine("Running test now");
            Program p = new Program();
            p.Run().GetAwaiter().GetResult();
            Debug.WriteLine("Finished test");
        }

        private async Task Run()
        {
            await SendAsync(HttpMethod.Get, "/api/ignorant");
            await SendAsync(HttpMethod.Get, "/api/reading");
            
            await SendAsync(HttpMethod.Post, "/api/reading", "{}");
            await SendAsync(HttpMethod.Post, "/api/ignorant", "{}");
        }

        private async Task<string> SendAsync(HttpMethod method, string path, string content = null)
        {
            try
            {
                Debug.WriteLine($"Request {method} {path}");
                var message = new HttpRequestMessage(method, new Uri(new Uri("http://localhost:8888"), path));
                if (content != null)
                {
                    message.Content = new StringContent(content, Encoding.UTF8, "application/json");;
                }

                var httpResponse = await _httpClient.SendAsync(message, HttpCompletionOption.ResponseContentRead,
                    CancellationToken.None);
                var result = await httpResponse.Content.ReadAsStringAsync();
                Debug.WriteLine($"Response to {path} was {(int)httpResponse.StatusCode} - {result}");
                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception thrown!");
                Debug.WriteLine(e);
                return null;
            }
        }
    }
}