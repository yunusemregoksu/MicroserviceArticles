using ArticleAPI.Settings;
using Microsoft.Extensions.Options;

namespace ArticleAPI.Services
{
    public class ReviewsServiceClient
    {
        private readonly HttpClient _httpClient;

        public ReviewsServiceClient(HttpClient httpClient, IOptions<ReviewApiSettings> reviewApiSettings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(reviewApiSettings.Value.BaseUrl);
        }

        public async Task<bool> HasReviewsForArticleAsync(string articleId)
        {
            using var response = await _httpClient.GetAsync($"api/Reviews/article/{articleId}/exists");
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return bool.TryParse(responseBody, out var hasReviews) && hasReviews;
        }
    }
}
