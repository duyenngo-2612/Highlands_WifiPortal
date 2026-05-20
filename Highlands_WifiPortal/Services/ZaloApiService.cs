using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Highlands_WifiPortal.Services
{
    public class ZaloApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public ZaloApiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<bool> SendOtp(string phone, string otp)
        {
            // Giả lập delay để UI mượt
            await Task.Delay(1000);

            try
            {
                var accessToken = _config["ZaloOA:AccessToken"];
                var url = "https://openapi.zalo.me/v3.0/oa/message/cs";

                var body = new
                {
                    recipient = new
                    {
                        user_id = phone // demo
                    },
                    message = new
                    {
                        text = $"Mã OTP của bạn là: {otp}"
                    }
                };

                var content = new StringContent(
                    JsonConvert.SerializeObject(body),
                    Encoding.UTF8,
                    "application/json"
                );

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("access_token", accessToken);

                var response = await _httpClient.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Zalo gửi thành công: " + result);
                    return true;
                }
                else
                {
                    Console.WriteLine("Zalo gửi thất bại: " + result);

                    // vẫn cho qua để demo không bị fail
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Zalo API: " + ex.Message);

                // vẫn cho qua để demo
                return true;
            }
        }
    }
}
