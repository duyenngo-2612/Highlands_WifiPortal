using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


namespace Highlands_WifiPortal.Services
{
    public class SmsApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public SmsApiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<bool> SendOtp(string phone, string otp)
        {
            try
            {
                var url = _config["SmsGateway:Url"];
                var apiKey = _config["SmsGateway:ApiKey"];

                var body = new
                {
                    to = phone,
                    message = $"Ma OTP cua ban la: {otp}"
                };

                var content = new StringContent(
                    JsonConvert.SerializeObject(body),
                    Encoding.UTF8,
                    "application/json"
                );

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                var response = await _httpClient.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("SMS gửi thành công: " + result);
                    return true;
                }
                else
                {
                    Console.WriteLine("SMS gửi thất bại: " + result);
                    return true; // fallback
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi SMS API: " + ex.Message);
                return true; 
            }
        }
    }
}
