using Newtonsoft.Json;
using Services.Configuration;
using Services.Interfaces;
using Services.Models;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class HttpSender : IMessageSender
    {
        private readonly HttpClient _client;
        private readonly string _url;

        public HttpSender(HttpClient httpClient, HttpSenderConfig config)
        {
            _client = httpClient;
            _url = config.CalculateNextRequestUrl;
        }

        public async Task SendMessage(FibonacciNumber message)
        {
            var json = JsonConvert.SerializeObject(message);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await _client.PostAsync(_url, content);
        }
    }
}
