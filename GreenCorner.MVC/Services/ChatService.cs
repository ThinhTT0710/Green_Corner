using System.Text.Json;
using System.Text;
using GreenCorner.MVC.Services.Interface;

namespace GreenCorner.MVC.Services
{
    public class ChatService : IChatService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public ChatService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<string> GetGeminiResponseAsync(string message)
        {
            string apiKey = _config["Gemini:ApiKey"];
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}";

            var prompt = $"Bạn là một trợ lý ảo hỗ trợ người dùng tư vấn sản phẩm và sự kiện hiện có trên website GreenCorner. Câu hỏi: {message}";

            var requestBody = new
            {
                contents = new[]
                {
                    new {
                        role = "user",
                        parts = new[] { new { text = prompt } }
                    }
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                return "Xin lỗi, hệ thống đang gặp sự cố. Vui lòng thử lại sau.";

            var result = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(result);
            var reply = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return reply ?? "Xin lỗi, tôi chưa hiểu rõ câu hỏi.";
        }
    }
}
