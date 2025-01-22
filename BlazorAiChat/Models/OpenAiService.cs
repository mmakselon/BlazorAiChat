namespace BlazorAiChat.Models
{
    public class OpenAiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OpenAiService(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
        }

        public async Task<string> GetAnswerAsync(string userPrompt)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization",$"Bearer {_apiKey}");

            var requestBody = new
            {
                model = "gpt-4",
                messages = new[]
                {
                    new {role="system",content = "Jesteś pomocnym asystentem."},
                    new { role = "user", content = userPrompt }
                },
                max_tokens = 100,
                temperature = 0.7
            };
            using var response = await _httpClient
                .PostAsJsonAsync("https://api.openai.com/v1/chat/completions", requestBody);

            if (!response.IsSuccessStatusCode)
            {
                return "Przepraszam, nie udało się uzyskać odpowiedzi od AI";
            }

           var jsonResponse = await response.Content.ReadFromJsonAsync<ChatCompletionResponse>();

            var answer = jsonResponse?.Choices?.FirstOrDefault()?.Message?.Content?.Trim();

            return string.IsNullOrWhiteSpace(answer) ? "Brak odpowiedzi" : answer;
        }
    }
}
