using Microsoft.Extensions.Options;
using ReviewAPI.Settings;

namespace ReviewAPI.Services
{
    public class ArticlesServiceClient
    {
        private readonly HttpClient _httpClient;

        public ArticlesServiceClient(HttpClient httpClient, IOptions<ArticleApiSettings> articleApiSettings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(articleApiSettings.Value.BaseUrl);
        }

        public async Task<bool> ArticleExistsAsync(string articleId)
        {
            using var response = await _httpClient.GetAsync($"api/Articles/{articleId}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }

            response.EnsureSuccessStatusCode();
            return true;
        }
    }
}
