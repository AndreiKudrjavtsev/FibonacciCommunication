using DTO;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class HttpSenderService
    {
        private readonly HttpClient _httpClient;
        private readonly string _secondServiceRequestUrl = "http://localhost:5001/fibonacci/next";

        public HttpSenderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendNextRequest(RequestDto request)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await _httpClient.PostAsync(_secondServiceRequestUrl, content);
        }

    }
}
