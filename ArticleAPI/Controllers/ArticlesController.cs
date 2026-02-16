using ArticleAPI.Entities;
using ArticleAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace ArticleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly ArticlesService _articlesService;
        private readonly ReviewsServiceClient _reviewsServiceClient;

        public ArticlesController(ArticlesService articlesService, ReviewsServiceClient reviewsServiceClient)
        {
            _articlesService = articlesService;
            _reviewsServiceClient = reviewsServiceClient;
        }

        [HttpGet]
        public async Task<List<Article>> Get() =>
            await _articlesService.GetAsync();


        [EnableQuery]
        [HttpGet("/odata/Articles")]
        public IQueryable<Article> GetOData() =>
            _articlesService.AsQueryable();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Article>> Get(string id)
        {
            var article = await _articlesService.GetAsync(id);

            if (article is null)
            {
                return NotFound();
            }

            return article;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Article newArticle)
        {
            await _articlesService.CreateAsync(newArticle);

            return CreatedAtAction(nameof(Get), new { id = newArticle.Id }, newArticle);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Article updatedArticle)
        {
            var article = await _articlesService.GetAsync(id);

            if (article is null)
            {
                return NotFound();
            }

            updatedArticle.Id = article.Id;

            await _articlesService.UpdateAsync(id, updatedArticle);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var article = await _articlesService.GetAsync(id);

            if (article is null)
            {
                return NotFound();
            }

            try
            {
                var hasReviews = await _reviewsServiceClient.HasReviewsForArticleAsync(id);

                if (hasReviews)
                {
                    return BadRequest($"Article with id '{id}' cannot be deleted because it has reviews.");
                }
            }
            catch (HttpRequestException)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Could not validate reviews for article.");
            }

            await _articlesService.RemoveAsync(id);

            return NoContent();
        }
    }
}
